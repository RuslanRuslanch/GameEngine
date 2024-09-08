using OpenTK.Graphics.OpenGL;

namespace GameEngine.Scripts.Buffers
{
    public sealed class VBO : Buffer
    {
        public unsafe void Generate<T>(T[] data) where T : unmanaged
        {
            int index = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, index);
            GL.BufferData<T>(BufferTarget.ArrayBuffer, new IntPtr(data.Length * sizeof(T)), data, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            Index = index;
        }

        public override void Delete()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(Index);
        }
    }
}
