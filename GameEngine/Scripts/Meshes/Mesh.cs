using GameEngine.Scripts.Buffers;
using OpenTK.Mathematics;

namespace GameEngine.Scripts.Meshes
{
    public class Mesh
    {
        public Polygon[] Polygons { get; private set; }

        public int PolygonCount { get; private set; }
        public VAO VAO { get; private set; }

        public void SetPolygons(Polygon[] polygons)
        {
            VBO vbo = new VBO();
            VAO = new VAO();

            List<Vector3> vertices = new List<Vector3>();

            foreach (Polygon polygon in polygons)
            {
                vertices.AddRange(polygon.Vertices);
            }

            vbo.Generate(vertices.ToArray());
            VAO.Generate(vbo);

            Polygons = polygons;
            PolygonCount = polygons.Length * 3;
        }
    }
}
