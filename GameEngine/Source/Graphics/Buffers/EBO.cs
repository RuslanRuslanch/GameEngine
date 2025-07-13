using OpenTK.Graphics.OpenGL;

namespace GameEngine.Resources
{
    public sealed class EBO
    {
        public readonly int ID;

        public EBO(uint[] indecies, BufferUsageHint usageHint)
        {
            ID = Generate(indecies, usageHint);
        }

        private int Generate(uint[] data, BufferUsageHint usageHint)
        {
            var id = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, id);
            GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(float), data, usageHint);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            return id;
        }
    }
}
