using OpenTK.Mathematics;

namespace GameEngine.Scripts.Meshes
{
    public class Polygon
    {
        public readonly Vector3[] Vertices;
        public readonly Vector2[] Uvs;
        public readonly Vector3 Normal;

        public Polygon(Vector3[] vertices, Vector2[] uvs, Vector3 normal)
        {
            Vertices = vertices.Length <= 3 ? vertices : throw new ArgumentOutOfRangeException(nameof(vertices.Length));
            //Uvs = uvs.Length <= 2 ? uvs : throw new ArgumentOutOfRangeException(nameof(uvs.Length));
            Normal = normal;
        }
    }
}
