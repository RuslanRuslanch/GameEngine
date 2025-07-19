using GameEngine.Graphics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Inputs
{
    public sealed class Input
    {
        public readonly KeyboardState Keyboard;
        public readonly MouseState Mouse;

        public Input(RenderWindow window)
        {
            Keyboard = window.KeyboardState;
            Mouse = window.MouseState;
        }

        public Input(KeyboardState keyboard, MouseState mouse)
        {
            Keyboard = keyboard;
            Mouse = mouse;
        }
    }
}