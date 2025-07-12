using GameEngine.Bootstraps;

namespace GameEngine
{
    public sealed class Program
    {
        private static void Main(string[] args)
        {
            var bootstraper = new CoreBootstraper();

            bootstraper.Initialize();
        }
    }
}
