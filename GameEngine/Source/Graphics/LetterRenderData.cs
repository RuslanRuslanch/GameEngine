using OpenTK.Mathematics;

namespace GameEngine.Graphics
{
    public class LetterRenderData
    {
        public readonly int VAO;
        public readonly Matrix4 ModelMatrix;

        public LetterRenderData(int vao, Vector3 position, Vector3 scale)
        {
            VAO = vao;

            ModelMatrix =
                Matrix4.CreateTranslation(position) *
                Matrix4.CreateScale(scale);
        }
    }
}
