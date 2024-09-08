using GameEngine.Scripts.Windowing;

namespace GameEngine.Scripts
{
    public sealed class Time
    {
        private static RenderWindow _window;

        public static double UpdateTime => _window.UpdateTime;

        public Time(RenderWindow window)
        {
            _window = window;
        }
    }
}
