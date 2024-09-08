using OpenTK.Windowing.Desktop;

namespace GameEngine.Scripts.Windowing.Debuging
{
    public class FPSCounterView
    {
        private readonly FPSCounter _counter;
        private readonly RenderWindow _window;

        public FPSCounterView(FPSCounter counter, RenderWindow window)
        {
            _counter = counter ?? throw new NullReferenceException(nameof(counter));
            _window = window ?? throw new NullReferenceException(nameof(window));

            _counter.Calculated += UpdateValue;
        }

        ~FPSCounterView()
        {
            _counter.Calculated -= UpdateValue;
        }

        private void UpdateValue(ushort value)
        {
            _window.Title = $"GameEngine | FPS: {value}";
        }
    }
}
