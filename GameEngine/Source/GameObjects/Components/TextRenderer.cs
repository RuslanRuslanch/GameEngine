using GameEngine.GameObjects;
using GameEngine.Graphics;
using GameEngine.Resources;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GameEngine.Components
{
    public sealed class TextRenderer : AbstractRenderer
    {
        private readonly HashSet<LetterRenderData> _renderDatas = new HashSet<LetterRenderData>();

        private Font _font;

        public TextRenderer(GameObject gameObject) : base(gameObject)
        {

        }

        public override void OnStart()
        {
            _font = GameObject.World.Core.Resource.Get<Font>("RussianFont");

            var material = GameObject.World.Core.Resource.Get<Material>("TextMaterial");
            var text = "1234567890";

            SetText(text);
            SetMaterial(material);
        }

        public override void OnRender()
        {
            GameObject.World.Core.Render.Bind(Material);

            foreach (var data in _renderDatas)
            {
                var model = data.ModelMatrix;

                Material.Shader.Load("model", ref model);

                GL.BindVertexArray(data.VAO);
                GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero);
                GL.BindVertexArray(0);
            }
        }

        public void SetText(string text)
        {
            _renderDatas.Clear();

            for (int i = 0; i < text.Length; i++)
            {
                var vao = _font.Get(text[i]);
                var letterPosition = new Vector3(_font.LetterSize.X * 16f, 0f, 0f) * i;
                var position = GameObject.Transform.Position + letterPosition;

                var renderData = new LetterRenderData(vao, position / 3f, Vector3.One * 0.1f);

                _renderDatas.Add(renderData);
            }
        }
    }
}
