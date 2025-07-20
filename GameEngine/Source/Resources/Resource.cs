namespace GameEngine.Resources
{
    public class Resource
    {
        private readonly HashSet<IResource> _resources = new HashSet<IResource>();

        public T Get<T>(string id) where T : IResource
        {
            var resource = _resources.FirstOrDefault(r => r.ID == id);

            if (resource == null)
            {
                throw new Exception("Resource not found");
            }

            return (T)resource;
        }

        public bool Has(string id)
        {
            return _resources.FirstOrDefault(r => r.ID == id) != null;
        }

        public void Save(IResource resource)
        {
            if (_resources.Contains(resource))
            {
                return;
            }

            _resources.Add(resource);
        }

        public IResource Load(string id, ResourceType type, params string[] paths)
        {
            if (type == ResourceType.Texture)
            {
                return new Texture(id, paths[0]);
            }
            else if (type == ResourceType.Shader)
            {
                return new Shader(id, paths[0], paths[1]);
            }
            else if (type == ResourceType.Sound)
            {
                return new Sound(id, paths[0]);
            }

            throw new Exception("This resource type is not supported now");
        }

        public IResource SaveAndLoad(string id, ResourceType type, params string[] paths)
        {
            var resource = Load(id, type, paths);

            Save(resource);

            return resource;
        }

        public void Delete(IResource resource)
        {
            resource.Delete();
        }
    }
}
