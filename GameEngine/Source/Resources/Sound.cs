using OpenTK.Audio.OpenAL;

namespace GameEngine.Resources
{
    public sealed class Sound : IResource
    {
        public ResourceType Type => ResourceType.Sound;

        public int Channels { get; private set; }
        public int BitsPerSample { get; private set; }
        public int SampleRate { get; private set; }
        public string ID { get; private set; }
        public int ALObject { get; private set; }

        public Sound(string id, string path)
        {
            ID = id;
            ALObject = Generate(path);
        }

        private int Generate(string path)
        {
            var wavBytes = File.ReadAllBytes(path);

            var (soundData, channels, bitsPerSample, sampleRate) = LoadWav(wavBytes);

            Channels = channels;
            BitsPerSample = bitsPerSample;
            SampleRate = sampleRate;

            int bufferId = AL.GenBuffer();

            ALFormat format = GetFormat(channels, bitsPerSample);

            AL.BufferData<byte>(bufferId, format, soundData, sampleRate);

            return bufferId;
        }

        private (byte[] soundData, int channels, int bitsPerSample, int sampleRate) LoadWav(byte[] wavBytes)
        {
            using (var stream = new MemoryStream(wavBytes))
            
            using (var reader = new BinaryReader(stream))
            {
                // Читаем заголовок RIFF
                var chunkID = new string(reader.ReadChars(4));

                if (chunkID != "RIFF")
                {
                    throw new Exception("Некорректный формат WAV");
                }

                reader.ReadInt32();

                var format = new string(reader.ReadChars(4));

                if (format != "WAVE")
                {
                    throw new Exception("Некорректный формат WAV");
                }

                // Читаем чанки до data
                var channels = 0;
                var sampleRate = 0;
                var bitsPerSample = 0;
                var soundData = new byte[1];

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var subChunkID = new string(reader.ReadChars(4));
                    var subChunkSize = reader.ReadInt32();

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
                {
                    throw new Exception("Не удалось найти данные звука");
                }

                return (soundData, channels, bitsPerSample, sampleRate);
            }
        }

        private ALFormat GetFormat(int channels, int bitsPerSample)
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

        public void Delete()
        {
            AL.DeleteBuffer(ALObject);
        }

    }
}