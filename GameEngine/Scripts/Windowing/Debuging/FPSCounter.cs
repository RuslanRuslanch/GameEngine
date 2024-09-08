namespace GameEngine.Scripts.Windowing.Debuging
{
    public sealed class FPSCounter
    {
        private ushort _currentFPS;
        private double _frameTime;

        public event Action<ushort> Calculated;

        public void Calculate()
        {
            if (_frameTime <= 1d)
            {
                _frameTime += Time.UpdateTime;
                _currentFPS++;
            }
            else
            {
                Calculated?.Invoke(_currentFPS);

                _frameTime = 0d;
                _currentFPS = 0;
            }
        }
    }
}

