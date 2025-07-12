using GameEngine.GameObjects;
using GameEngine.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GameEngine.Components
{
    public sealed class LineRenderer : Component
    {
        public Vector3[] Points { get; private set; }
        public Color4 Color { get; private set; } = Color4.ForestGreen;
        public float Size { get; private set; } = 5f;
        
        private Shader _shader;

        private int _vao;
        private int _vertexObject;

        public LineRenderer(GameObject gameObject) : base(gameObject)
        {
        }

        public override void OnStart()
        {
            SetInitialized();

            _shader = Resources.Resources.Get<Shader>("lineShader");

            CreateBuffers();
        }

        public override void OnFinish()
        {
            DeleteBuffers();
        }

        public override void OnPreRender()
        {
            var projection = GameObject.World.MainCamera.ProjectionMatrix;
            var view = GameObject.World.MainCamera.ViewMatrix;
            var model = GameObject.Transform.ModelMatrix;
            var color = Color;

            _shader.Load("projection", ref projection);
            _shader.Load("view", ref view);
            _shader.Load("model", ref model);
            _shader.Load("color", ref color);
        }

        public override void OnRender()
        {
            _shader.Bind();

            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.Lines, 0, Points.Length);
        }

        public void SetColor(Color4 color)
        {
            Color = color;
        }
        
        public void SetSize(float size)
        {
            Size = size;
        }

        public void SetPoints(Vector3[] points)
        {
            if (
                points.Length % 2 != 0 ||
                points.Length == 0)
            {
                throw new Exception("Количество точек долго быть четное, и не равно нулю");
            }

            Points = points;

            RecalculateBuffers();
        }

        public void RecalculateBuffers()
        {
            DeleteBuffers();
            CreateBuffers();
        }

        private void CreateBuffers()
        {
            if (IsInitialized == false)
            {
                return;
            }

            var vertexObject = new VBO(Points, BufferUsageHint.StaticDraw).ID;
            var vao = new LineVAO(vertexObject, _shader).ID;

            _vertexObject = vertexObject;
            _vao = vao;
        }

        public void DeleteBuffers()
        {
            if (_vao == 0)
            {
                return;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexObject);

            GL.BindVertexArray(0);
            GL.DeleteVertexArray(_vao);
        }
    }
}
