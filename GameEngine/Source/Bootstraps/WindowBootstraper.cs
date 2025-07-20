using GameEngine.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace GameEngine.Bootstraps
{
    public sealed class WindowBootstraper
    {
        public void Initialize()
        {
            var gameSettings = new GameWindowSettings();
            var nativeSettings = new NativeWindowSettings();

            nativeSettings.APIVersion = new Version(3, 3);
            nativeSettings.ClientSize = new Vector2i(1280, 720);

            using (var window = new RenderWindow(gameSettings, nativeSettings))
            {
                window.CenterWindow();

                window.Run();
            }
        }
    }
}