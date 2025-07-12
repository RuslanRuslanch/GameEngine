using System.IO;
using OpenTK.Audio.OpenAL;

namespace GameEngine.Sounds
{
    public sealed class Sound
    {
        public readonly int ID;

        public int Channels { get; private set; }
        public int BitsPerSample { get; private set; }
        public int SampleRate { get; private set; }

        public Sound(string path)
        {
            ID = Generate(path);
        }

        private int Generate(string path)
        {
            // Загружаем WAV файл
            byte[] wavBytes = File.ReadAllBytes(path);
            // Парсим WAV
            var (soundData, channels, bitsPerSample, sampleRate) = LoadWav(wavBytes);

            Channels = channels;
            BitsPerSample = bitsPerSample;
            SampleRate = sampleRate;

            int bufferId = AL.GenBuffer();

            ALFormat format = GetALFormat(channels, bitsPerSample);

            AL.BufferData<byte>(bufferId, format, soundData, sampleRate);

            return bufferId;
        }

        private (byte[] soundData, int channels, int bitsPerSample, int sampleRate) LoadWav(byte[] wavBytes)
        {
            using (var ms = new MemoryStream(wavBytes))
            using (var reader = new BinaryReader(ms))
            {
                // Читаем заголовок RIFF
                string chunkID = new string(reader.ReadChars(4));
                if (chunkID != "RIFF")
                {
                    throw new Exception("Некорректный формат WAV");
                }

                reader.ReadInt32(); // Размер файла
                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                {
                    throw new Exception("Некорректный формат WAV");
                }

                // Читаем чанки до data
                int channels = 0;
                int sampleRate = 0;
                int bitsPerSample = 0;
                byte[] soundData = null;

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    string subChunkID = new string(reader.ReadChars(4));
                    int subChunkSize = reader.ReadInt32();

                    if (subChunkID == "fmt ")
                    {
                        short audioFormat = reader.ReadInt16();
                        channels = reader.ReadInt16();
                        sampleRate = reader.ReadInt32();
                        reader.ReadInt32(); // Byte rate
                        reader.ReadInt16(); // Block align
                        bitsPerSample = reader.ReadInt16();

                        // Если есть дополнительные параметры - пропускаем их
                        if (subChunkSize > 16)
                            reader.ReadBytes(subChunkSize - 16);
                    }
                    else if (subChunkID == "data")
                    {
                        soundData = reader.ReadBytes(subChunkSize);
                        break; // нашли данные - выходим
                    }
                    else
                    {
                        // пропускаем неизвестные чанки
                        reader.ReadBytes(subChunkSize);
                    }
                }

                if (soundData == null)
                    throw new Exception("Не удалось найти данные звука");

                return (soundData, channels, bitsPerSample, sampleRate);
            }
        }

        private ALFormat GetALFormat(int channels, int bitsPerSample)
        {
            if (channels == 1 && bitsPerSample == 8)
            {
                return ALFormat.Mono8;
            }
            else if (channels == 2 && bitsPerSample == 8)
            {
                return ALFormat.Stereo8;
            }
            else if (channels == 1 && bitsPerSample == 16)
            {
                return ALFormat.Mono16;
            }
            else if (channels == 2 && bitsPerSample == 16)
            {
                return ALFormat.Stereo16;
            }

            throw new Exception("Неудалось определить формат звука");
        }
    }
}