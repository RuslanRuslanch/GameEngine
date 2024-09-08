using GameEngine.Scripts.Windowing;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace GameEngine.Scripts
{
    public sealed class Program
    {
        private static void Main(string[] args)
        {
            Vector2i windowSize = new Vector2i(1270, 720);
            string windowTitle = "GameEngine";

            GameWindowSettings gameSettings = new GameWindowSettings();
            NativeWindowSettings nativeSettings = new NativeWindowSettings();

            nativeSettings.Profile = ContextProfile.Compatability;
            nativeSettings.ClientSize = windowSize;
            nativeSettings.Title = windowTitle;

            using (RenderWindow window = new RenderWindow(gameSettings, nativeSettings))
            {
                window.Run();
            }
        }
    }
}
