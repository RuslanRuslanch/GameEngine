namespace GameEngine.Debugs
{
    public sealed class Gizmos
    {
        public bool IsEnabled { get; private set; }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }
    }
}