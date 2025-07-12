using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Inputs
{
    public static class Input
    {
        public static KeyboardState Keyboard;
        public static MouseState Mouse;

        public static void Initialize(KeyboardState keyboard, MouseState mouse)
        {
            Keyboard = keyboard;
            Mouse = mouse;
        }
    }
}