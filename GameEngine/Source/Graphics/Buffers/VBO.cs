using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GameEngine.Resources
{
    public sealed class VBO
    {
        public readonly int ID;

        public VBO(float[] data, BufferUsageHint usageHint)
        {
            ID = Generate(data, usageHint);
        }

        public VBO(Vector3[] data, BufferUsageHint usageHint)
        {
            ID = Generate(data, usageHint);
        }

        public VBO(Color4[] data, BufferUsageHint usageHint)
        {
            ID = Generate(data, usageHint);
        }

        public VBO(Vector2[] data, BufferUsageHint usageHint)
        {
            ID = Generate(data, usageHint);
        }

        private int Generate(float[] data, BufferUsageHint usageHint)
        {
            var id = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, id);
            GL.BufferData<float>(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, usageHint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            return id;
        }

        private int Generate(Vector3[] data, BufferUsageHint usageHint)
        {
            var id = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, id);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, data.Length * Vector3.SizeInBytes, data, usageHint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            return id;
        }

        private int Generate(Color4[] data, BufferUsageHint usageHint)
        {
            var id = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, id);
            GL.BufferData<Color4>(BufferTarget.ArrayBuffer, data.Length * sizeof(float) * 4, data, usageHint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            return id;
        }

        private int Generate(Vector2[] data, BufferUsageHint usageHint)
        {
            var id = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, id);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, data.Length * Vector2.SizeInBytes, data, usageHint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            return id;
        }
    }
}
