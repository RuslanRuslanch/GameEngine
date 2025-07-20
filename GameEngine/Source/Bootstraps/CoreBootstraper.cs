namespace GameEngine.Bootstraps
{
    public sealed class CoreBootstraper
    {
        public void Initialize()
        {
            var window = new WindowBootstraper();

            window.Initialize();
        }
    }
}
