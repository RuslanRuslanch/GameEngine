using GameEngine.Resources;

namespace GameEngine.Graphics
{
    public sealed class RenderSystem
    {
        private Texture _texture;
        private Shader _shader;

        public void Bind(Material material)
        {
            Bind(material.Shader);
            Bind(material.Texture);
        }

        public void Bind(Shader shader)
        {
            if (_shader == shader)
            {
                return;
            }

            shader.Bind();

            _shader = shader;
        }

        public void Bind(Texture texture)
        {
            if (_texture == texture)
            {
                return;
            }

            texture.Bind();

            _texture = texture;
        }
    }
}