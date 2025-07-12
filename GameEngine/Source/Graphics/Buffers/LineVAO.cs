using OpenTK.Graphics.OpenGL;

namespace GameEngine.Graphics
{
    public sealed class LineVAO : VAO
    {
        public LineVAO(int vertexObject, Shader shader) : base(shader)
        {
            ID = Generate(vertexObject, shader);
        }

        private int Generate(int vertexObject, Shader shader)
        {
            var id = GL.GenVertexArray();

            var positionIndex = shader.GetLocation("vPosition");

            GL.BindVertexArray(id);

            GL.EnableVertexAttribArray(positionIndex);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexObject);
            GL.VertexAttribPointer(positionIndex, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.DisableVertexAttribArray(positionIndex);

            return id;
        }
    }
}
