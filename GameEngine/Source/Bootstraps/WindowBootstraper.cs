using GameEngine.Graphics;
using OpenTK.Windowing.Desktop;

namespace GameEngine.Bootstraps
{
    public sealed class WindowBootstraper
    {
        public void Initialize()
        {
            var gameSettings = new GameWindowSettings();
            var nativeSettings = new NativeWindowSettings();

            using (var window = new RenderWindow(gameSettings, nativeSettings))
            {
                window.Run();
            }
        }

        public void Deinitialize()
        {
            
        }
    }
}