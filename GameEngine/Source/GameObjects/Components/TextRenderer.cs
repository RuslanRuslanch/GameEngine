using GameEngine.GameObjects;
using GameEngine.Graphics;
using GameEngine.Resources;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GameEngine.Components
{
    public sealed class TextRenderer : Component
    {
        private readonly HashSet<LetterRenderData> _renders = new HashSet<LetterRenderData>();

        private Font _font;

        public TextRenderer(GameObject gameObject) : base(gameObject)
        {
            
        }

        public override void OnStart()
        {
            _font = GameObject.World.Core.Resource.Get<Font>("RussianFont");

            var text = "11111";

            for (int i = 0; i < text.Length; i++)
            {
                var vao = _font.Get(text[i]);
                var letterPosition = new Vector3(_font.LetterSize.X, 0f, 0f) * i;
                var position = GameObject.Transform.Position + letterPosition;

                var renderData = new LetterRenderData(vao, position, Vector3.One * 0.1f);

                _renders.Add(renderData);
            }
        }

        public override void OnPreRender()
        {
            var projection = GameObject.World.MainCamera.ProjectionMatrix;
            var view = GameObject.World.MainCamera.ViewMatrix;

            _font.Shader.Load("projection", ref projection);
            _font.Shader.Load("view", ref view);
        }

        public override void OnRender()
        {
            GameObject.World.Core.Render.Bind(_font.Texture);
            GameObject.World.Core.Render.Bind(_font.Shader);

            foreach (var data in _renders)
            {
                var model = data.ModelMatrix;

                _font.Shader.Load("model", ref model);

                GL.BindVertexArray(data.VAO);
                GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero);
                GL.BindVertexArray(0);
            }
        }
    }
}
