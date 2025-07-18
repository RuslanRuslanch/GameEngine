using GameEngine.GameObjects;

namespace GameEngine.Resources
{
    public sealed class Prefab : IResource
    {
        public ResourceType Type => ResourceType.Prefab;
        public string ID { get; private set; }
        
        public GameObject GameObject { get; private set; }

        public Prefab(string id, GameObject gameObject)
        {
            ID = id;
            GameObject = gameObject;
        }

        public void Delete()
        {
            return;
        }
    }
}