using GameEngine.Resources;

namespace GameEngine.Graphics
{
    public sealed class RenderSystem
    {
        private int _texture;
        private int _shader;

        public void Bind(Material material)
        {
            Bind(material.Shader);
            Bind(material.Texture);
        }

        public void Bind(Shader shader)
        {
            if (_shader == shader.GLObject)
            {
                return;
            }

            shader.Bind();

            _shader = shader.GLObject;
        }

        public void Bind(Texture texture)
        {
            if (_texture == texture.GLObject)
            {
                return;
            }

            texture.Bind();

            _texture = texture.GLObject;
        }
    }
}