using GameEngine.Graphics;
using GameEngine.Resources;
using OpenTK.Mathematics;

namespace GameEngine.Bootstraps
{
    public sealed class ResourceBootstraper
    {
        public void Initialize()
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

            Resources.Resources.Save(new Mesh("spriteMesh", vertices, normals, indecies));
            Resources.Resources.SaveAndLoad("lineShader", ResourceType.Shader, new[] { @"Shaders\LineWorldVertexShader.vert", @"Shaders\LineWorldFragmentShader.frag" });
            Resources.Resources.SaveAndLoad("spriteShader", ResourceType.Shader, new[] { @"Shaders\SpriteVertexShader.vert", @"Shaders\SpriteFragmentShader.frag" });
            Resources.Resources.SaveAndLoad("testTexture", ResourceType.Texture, new[] { @"Textures\Texture.png" });
        }

        public void Deinitialize()
        {
            
        }
    }
}