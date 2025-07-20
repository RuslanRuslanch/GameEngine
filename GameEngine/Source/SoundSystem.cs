using OpenTK.Audio.OpenAL;

namespace GameEngine.Sounds
{
    public sealed class SoundSystem : IDisposable
    {
        private readonly ALDevice _device;

        public SoundSystem()
        {
            _device = ALC.OpenDevice(null);

            unsafe
            {
                var context = ALC.CreateContext(_device, (int*)IntPtr.Zero);
                ALC.MakeContextCurrent(context);
            }
        }

        public void Dispose()
        {
            ALC.CloseDevice(_device);
        }
    } 
}   