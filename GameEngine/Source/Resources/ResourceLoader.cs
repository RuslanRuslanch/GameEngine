using OpenTK.Mathematics;

namespace GameEngine.Resources
{
    public sealed class ResourceLoader
    {
        public void Initialize(Resource resource)
        {
            var vertices = new Vector3[]
            {
                Vector3.Zero,
                Vector3.UnitX,
                Vector3.UnitY,
                new Vector3(1f, 1f, 0f),
            };

            var uvs = new Vector2[]
            {
                Vector2.Zero,
                Vector2.UnitX,
                Vector2.UnitY,
                Vector2.One,
            };

            var indecies = new uint[]
            {
                0, 1, 3,
                0, 3, 2
            };

            var normals = new Vector3[]
            {
                -Vector3.UnitZ,
                -Vector3.UnitZ,
            };

            var texture = resource.Get<Texture>("RussianFontTexture");
            var shader = resource.Get<Shader>("UIShader");

            var size = new Vector2(16f / texture.Width, 16f / texture.Height);
            var font = new Font("RussianFont", "1", texture, shader, size);

            resource.Save(new Mesh("SpriteMesh", vertices, uvs, normals, indecies));
            resource.Save(font);
            
            font.Generate(resource);
        }

        public void Deinitialize()
        {
            
        }
    }
}