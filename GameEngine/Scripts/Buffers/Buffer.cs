namespace GameEngine.Scripts.Buffers
{
    public abstract class Buffer
    {
        public int Index { get; protected set; }

        public abstract void Delete();
    }
}
