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
        }

        public void Deinitialize()
        {
            
        }
    }
}