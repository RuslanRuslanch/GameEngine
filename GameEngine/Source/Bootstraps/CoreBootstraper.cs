namespace GameEngine.Bootstraps
{
    public sealed class CoreBootstraper
    {
        public void Initialize()
        {
            var window = new WindowBootstraper();
            var sound = new SoundBootstraper();

            sound.Initialize();
            window.Initialize();
        }
    }
}
