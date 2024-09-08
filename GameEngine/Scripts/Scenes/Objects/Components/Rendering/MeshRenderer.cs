using GameEngine.Scripts.Meshes;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Scripts.Scenes.Objects.Components.Rendering
{
    public sealed class MeshRenderer : Renderer
    {
        public Mesh Mesh { get; private set; }

        public MeshRenderer(Mesh mesh)
        {
            SetMesh(mesh);
        }

        public override void Draw()
        {
            GL.BindVertexArray(Mesh.VAO.Index);
            GL.DrawArrays(PrimitiveType.Triangles, 0, Mesh.PolygonCount);
        }

        public void SetMesh(Mesh mesh) 
        {
            if (mesh == null)
            {
                throw new NullReferenceException(nameof(mesh));
            }

            Mesh = mesh;
        }
    }
}
