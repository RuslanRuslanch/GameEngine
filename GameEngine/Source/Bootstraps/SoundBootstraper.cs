using OpenTK.Audio.OpenAL;

namespace GameEngine.Bootstraps
{
    public sealed class SoundBootstraper
    {
        public unsafe void Initialize()
        {
            var device = ALC.OpenDevice(null);
            var context = ALC.CreateContext(device, (int*)IntPtr.Zero);
            
            ALC.MakeContextCurrent(context);
        }
    } 
}