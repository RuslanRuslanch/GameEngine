using GameEngine.GameObjects;
using GameEngine.Graphics;
using GameEngine.Resources;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GameEngine.Components
{
    public sealed class LineRenderer : Component
    {
        public Vector3[] Points { get; private set; } = Array.Empty<Vector3>();
        public Color4 Color { get; private set; } = Color4.ForestGreen;
        public float Size { get; private set; } = 5f;

        private Shader _shader;
        private AABB _aabb;

        private int _vao;
        private int _vertexObject;

        public LineRenderer(GameObject gameObject) : base(gameObject)
        {
        }

        public override void OnStart()
        {
            base.OnStart();

            _shader = GameObject.World.Core.Resource.Get<Shader>("LineShader");

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
            GameObject.World.Core.Render.Bind(_shader);

            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.Lines, 0, Points.Length);
            GL.BindVertexArray(0);
        }

        public override bool CanRender(Frustum frustum)
        {
            return frustum.InFrustum(_aabb);
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
            RecalculateAABB();
        }

        public void AddPoints(Vector3[] newPoints)
        {
            if (
                newPoints.Length % 2 != 0 ||
                newPoints.Length == 0)
            {
                throw new Exception("Количество точек долго быть четное, и не равно нулю");
            }

            var points = Points.ToList();

            points.AddRange(newPoints);

            Points = points.ToArray();

            RecalculateBuffers();
            RecalculateAABB();
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

            var vertexObject = new VBO(Points.ToArray(), BufferUsageHint.StaticDraw).ID;
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

        public void RecalculateAABB()
        {
            var (min, max) = GetAABBSize();
            var position = GameObject.Transform.Position;

            _aabb = new AABB(position + min, position + max);
        }

        private (Vector3 min, Vector3 max) GetAABBSize()
        {
            var min = Points[0];
            var max = Points[0];

            foreach (var point in Points)
            {
                if (point.X < min.X || point.Y < min.Y || point.Z < min.Z)
                {
                    min = point;
                }

                if (point.X > max.X || point.Y > max.Y || point.Z > max.Z)
                {
                    max = point;
                }
            }

            return (min, max);
        }
    }
}
