using GameEngine.Resources;
using OpenTK.Mathematics;

namespace GameEngine.Graphics
{
    public class Mesh : IResource
    {
        public ResourceType Type => ResourceType.Mesh;
        public int GLObject => throw new Exception("Mesh hasn't OpenGL object");

        public Vector3[] Vertices { get; private set; }
        public Vector2[] UVs { get; private set; }
        public Vector3[] Normals { get; private set; }
        public uint[] Indecies { get; private set; }

        public string ID { get; private set; }

        public Mesh(string id, Mesh mesh, Vector2[] uvs)
        {
            ID = id;
            Vertices = mesh.Vertices;
            Normals = mesh.Normals;
            Indecies = mesh.Indecies;
            UVs = uvs;
        }

        public Mesh(string id, Vector3[] vertices, Vector2[] uVs, Vector3[] normals, uint[] indecies)
        {
            ID = id;
            Vertices = vertices;
            UVs = uVs;
            Normals = normals;
            Indecies = indecies;
        }

        public Mesh(string id, Vector3[] vertices, Vector2[] uVs, uint[] indecies)
        {
            ID = id;
            Vertices = vertices;
            UVs = uVs;
            Normals = Array.Empty<Vector3>();
            Indecies = indecies;
        }

        public Mesh(string id, Vector3[] vertices, Vector3[] normals, uint[] indecies)
        {
            ID = id;
            Vertices = vertices;
            UVs = Array.Empty<Vector2>();
            Normals = normals;
            Indecies = indecies;
        }

        public Mesh(string id, Vector3[] vertices, uint[] indecies)
        {
            ID = id;
            Vertices = vertices;
            UVs = Array.Empty<Vector2>();
            Normals = Array.Empty<Vector3>();
            Indecies = indecies;
        }

        public void SetUVs(Vector2[] uvs)
        {
            UVs = uvs;
        }

        public void SetVertices(Vector3[] vertices)
        {
            Vertices = vertices;
        }

        public void SetNormals(Vector3[] normals)
        {
            Normals = normals;
        }

        public void SetNormals(uint[] indecies)
        {
            Indecies = indecies;
        }
    }
}
