using GameEngine.Resources;
using OpenTK.Graphics.OpenGL;

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
            if (resource.Type == ResourceType.Texture)
            {
                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.DeleteTexture(resource.GLObject);
            }
            else if (resource.Type == ResourceType.Shader)
            {
                GL.UseProgram(0);
                GL.DeleteProgram(resource.GLObject);
            }
            else
            {
                throw new Exception("This resource type is not supported now");
            }

            resource = null;
        }
    }
}
