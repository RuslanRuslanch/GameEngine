namespace GameEngine.Resources
{
    public sealed class Material : IResource
    {
        public readonly Texture Texture;
        public readonly Shader Shader;
        
        public ResourceType Type => ResourceType.Material;
        public string ID { get; private set; }

        public Material(string id, Texture texture, Shader shader)
        {
            ID = id;
            Texture = texture;
            Shader = shader;
        }

        public void Delete()
        {
            Texture.Delete();
            Shader.Delete();
        }
    }
}
