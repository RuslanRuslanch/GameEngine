namespace GameEngine.Resources
{
    public interface IResource
    {
        public ResourceType Type { get; }
        public string ID { get; }

        public void Delete();
    }
}
