using GameEngine.Resources;
using GameEngine.Resources;
using OpenTK.Mathematics;

namespace GameEngine.Bootstraps
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

            resource.Save(new Mesh("spriteMesh", vertices, normals, indecies));
            resource.SaveAndLoad("lineShader", ResourceType.Shader, @"Shaders\LineWorldVertexShader.vert", @"Shaders\LineWorldFragmentShader.frag");
            resource.SaveAndLoad("spriteShader", ResourceType.Shader, @"Shaders\SpriteVertexShader.vert", @"Shaders\SpriteFragmentShader.frag" );
            resource.SaveAndLoad("dirtTexture", ResourceType.Texture, @"Textures\Dirt.png" );
            resource.SaveAndLoad("characterTexture", ResourceType.Texture, @"Textures\Character.png" );
        }

        public void Deinitialize()
        {
            
        }
    }
}