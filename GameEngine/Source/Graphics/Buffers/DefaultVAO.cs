using OpenTK.Graphics.OpenGL;

namespace GameEngine.Graphics
{
    public sealed class DefaultVAO : VAO
    {
        public DefaultVAO(int ebo, int vertexObject, int uvObject, int normalObject, Shader shader) : base(shader)
        {
            ID = Generate(ebo, vertexObject, uvObject, normalObject, shader);
        }

        private int Generate(int ebo, int vertexObject, int uvObject, int normalObject, Shader shader)
        {
            var id = GL.GenVertexArray();

            var positionIndex = shader.GetLocation("vPosition");
            var uvIndex = shader.GetLocation("vUV");
            var normalIndex = shader.GetLocation("vNormal");

            GL.BindVertexArray(id);

            GL.EnableVertexAttribArray(positionIndex);
            GL.EnableVertexAttribArray(uvIndex);
            GL.EnableVertexAttribArray(normalIndex);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexObject);
            GL.VertexAttribPointer(positionIndex, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, uvObject);
            GL.VertexAttribPointer(uvIndex, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, normalObject);
            GL.VertexAttribPointer(normalIndex, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.DisableVertexAttribArray(positionIndex);
            GL.DisableVertexAttribArray(uvIndex);
            GL.DisableVertexAttribArray(normalIndex);

            return id;
        }
    }
}
