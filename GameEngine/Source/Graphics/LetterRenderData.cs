using OpenTK.Mathematics;

namespace GameEngine.Graphics
{
    public struct LetterRenderData
    {
        public readonly int VAO;
        public readonly Vector3 Position;
        public readonly Matrix4 ModelMatrix;

        public LetterRenderData(int vao, Vector3 position, Vector3 scale)
        {
            VAO = vao;
            Position = position;
            ModelMatrix = Matrix4.CreateTranslation(position) * Matrix4.CreateScale(scale);
        }
    }
}
