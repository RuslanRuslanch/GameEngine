using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Scripts
{
    public class Input
    {
        private static KeyboardState _keyboard;
        private static MouseState _mouse;

        public static Vector2 MousePosition => _mouse.Position;
        public static Vector2 MouseDelta => _mouse.Delta;

        public Input(KeyboardState keyboard, MouseState mouse)
        {
            _keyboard = keyboard;
            _mouse = mouse;
        }

        public static bool IsKeyDown(Keys key)
        {
            return _keyboard.IsKeyDown(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return _keyboard.IsKeyPressed(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return _keyboard.IsKeyReleased(key);
        }

        public static bool MouseKeyPressed(MouseButton button)
        {
            return _mouse.IsButtonPressed(button);
        }
        public static bool MouseKeyDown(MouseButton button)
        {
            return _mouse.IsButtonDown(button);
        }
        public static bool MouseKeyReleased(MouseButton button)
        {
            return _mouse.IsButtonReleased(button);
        }
    }
}
