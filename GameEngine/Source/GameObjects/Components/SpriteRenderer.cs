using GameEngine.GameObjects;
using GameEngine.Graphics;
using GameEngine.Resources;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GameEngine.Components
{
    public sealed class SpriteRenderer : AbstractRenderer
    {
        private int _vao;
        private int _vertexObject;
        private int _uvObject;
        private int _normalObject;
        private int _ebo;

        private UVRegion _uvRegion = new UVRegion();

        public SpriteRenderer(GameObject gameObject) : base(gameObject)
        {
        }

        public override void OnStart()
        {
            base.OnStart();

            var material = GameObject.World.Core.Resource.Get<Material>("SpriteMaterial");

            SetMaterial(material);
            CreateBuffers();
        }

        public override void OnFinish()
        {
            DeleteBuffers();
        }

        public override bool CanRender(Frustum frustum)
        {
            var aabb = new AABB(GameObject.Transform.Position, GameObject.Transform.Position + GameObject.Transform.Scale);

            return frustum.InFrustum(aabb);
        }

        public override void OnPreRender()
        {
            var projection = GameObject.World.MainCamera.ProjectionMatrix;
            var view = GameObject.World.MainCamera.ViewMatrix;
            var model = GameObject.Transform.ModelMatrix;

            Material.Shader.Load("projection", ref projection);
            Material.Shader.Load("view", ref view);
            Material.Shader.Load("model", ref model);
        }

        public override void OnRender()
        {
            GameObject.World.Core.Render.Bind(Material);

            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);
        }

        public void SetUV(UVRegion region)
        {
            _uvRegion = region;

            DeleteBuffers();
            CreateBuffers();
        }

        private void CreateBuffers()
        {
            var mesh = GameObject.World.Core.Resource.Get<Mesh>("SpriteMesh");

            var uvs = new Vector2[]
            {
                _uvRegion.Min,
                _uvRegion.XOffset,
                _uvRegion.YOffset,
                _uvRegion.Max,
            };

            _vertexObject = new VBO(mesh.Vertices, BufferUsageHint.StaticDraw).ID;
            _uvObject = new VBO(uvs, BufferUsageHint.StaticDraw).ID;
            _normalObject = new VBO(mesh.Normals, BufferUsageHint.StaticDraw).ID;
            _ebo = new EBO(mesh.Indecies, BufferUsageHint.StaticDraw).ID;

            _vao = new DefaultVAO(_ebo, _vertexObject, _uvObject, _normalObject, Material.Shader).ID;
        }

        private void DeleteBuffers()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(_ebo);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexObject);
            GL.DeleteBuffer(_uvObject);
            GL.DeleteBuffer(_normalObject);

            GL.BindVertexArray(0);
            GL.DeleteVertexArray(_vao);
        }
    }
}
