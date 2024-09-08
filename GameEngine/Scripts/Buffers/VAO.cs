using OpenTK.Graphics.OpenGL;

namespace GameEngine.Scripts.Buffers
{
    public sealed class VAO : Buffer
    {
        public void Generate(VBO data)
        {
            int index = GL.GenVertexArray();

            GL.BindVertexArray(index);

            GL.EnableClientState(ArrayCap.VertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, data.Index);
            GL.VertexPointer(3, VertexPointerType.Float, 0, 0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.DisableClientState(ArrayCap.VertexArray);

            Index = index;
        }

        public override void Delete()
        {
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(Index);
        }
    }
}
