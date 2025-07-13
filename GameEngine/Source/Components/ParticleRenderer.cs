using GameEngine.GameObjects;
using GameEngine.Resources;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Components
{
    public sealed class ParticleRenderer : Component
    {
        public bool IsStarted { get; private set; } = false;
        public int MaxParticleCount { get; private set; } = 0;
        public Material Material { get; private set; }

        private readonly List<Particle> _particles = new List<Particle>();
        private readonly UVRegion _uvRegion = new UVRegion();

        private int _vao;

        public ParticleRenderer(GameObject gameObject) : base(gameObject)
        {
        }

        public override void OnStart()
        {
            SetInitialized();

            var shader = GameObject.World.Core.Resource.Get<Shader>("spriteShader");
            var texture = GameObject.World.Core.Resource.Get<Texture>("testTexture");

            var material = new Material(texture, shader);

            Material = material;

            CreateBuffers();
        }

        public override void OnUpdate(float delta)
        {
            if (GameObject.World.Core.Input.Keyboard.IsKeyPressed(Keys.P))
            {
                Stop();
                SetMaxParticleCount(MaxParticleCount + 1000);
                Start();

                Console.WriteLine($"Current particle count: {MaxParticleCount}");
            }

            if (IsStarted == false)
            {
                return;
            }

            for (int i = 0; i < MaxParticleCount; i++)
            {
                _particles[i].Move(Vector3.UnitY * delta * 0f);
            }
        }

        public override void OnPreRender()
        {
            if (IsStarted == false)
            {
                return;
            }

            var projection = GameObject.World.MainCamera.ProjectionMatrix;
            var view = GameObject.World.MainCamera.ViewMatrix;

            Material.Shader.Load("projection", ref projection);
            Material.Shader.Load("view", ref view);
        }

        public override void OnRender()
        {
            if (IsStarted == false)
            {
                return;
            }

            Material.Shader.Bind();
            Material.Texture.Bind();

            GL.BindVertexArray(_vao);

            for (int i = 0; i < MaxParticleCount; i++)
            {
                var frustum = GameObject.World.MainCamera.Frustum;

                if (_particles[i].CanRender(frustum) == false)
                {
                    continue;
                }

                var model = _particles[i].ModelMatrix;

                Material.Shader.Load("model", ref model);
                
                GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }
        }

        public void Start()
        {
            IsStarted = true;

            for (int i = 0; i < MaxParticleCount; i++)
            {
                var particle = new Particle(GameObject.Transform.Position + Vector3.UnitX * i * 0.3f, 0.25f);

                _particles.Add(particle);
            }
        }

        public void Stop()
        {
            _particles.Clear();

            IsStarted = false;
        }

        public void SetMaxParticleCount(int count)
        {
            MaxParticleCount = count;
        }

        private void CreateBuffers()
        {
            var mesh = GameObject.World.Core.Resource.Get<Mesh>("spriteMesh");

            var uvs = new Vector2[]
            {
                _uvRegion.Min,
                _uvRegion.XOffset,
                _uvRegion.YOffset,
                _uvRegion.Max,
            };

            var vertexObject = new VBO(mesh.Vertices, BufferUsageHint.StaticDraw).ID;
            var uvObject = new VBO(uvs, BufferUsageHint.StaticDraw).ID;
            var normalObject = new VBO(mesh.Normals, BufferUsageHint.StaticDraw).ID;
            var ebo = new EBO(mesh.Indecies, BufferUsageHint.StaticDraw).ID;

            _vao = new DefaultVAO(ebo, vertexObject, uvObject, normalObject, Material.Shader).ID;
        }
    }

    public class Particle
    {
        public readonly float Scale;

        public Matrix4 ModelMatrix { get; private set; } = Matrix4.Identity;

        public Vector3 Position { get; private set; }

        public Particle(Vector3 position, float scale)
        {
            Position = position;
            Scale = scale;

            ReloadModelMatrix();
        }

        public void Move(float x, float y, float z)
        {
            Move(new Vector3(x, y, z));
        }

        public void Move(Vector3 velocity)
        {
            Position += velocity;

            ReloadModelMatrix();
        }

        private void ReloadModelMatrix()
        {
            var translation = Matrix4.CreateTranslation(Position);
            var scale = Matrix4.CreateScale(Scale, Scale, 1f);

            ModelMatrix =
                scale *
                translation;
        }

        public bool CanRender(Frustum frustum)
        {
            var aabb = new AABB(Position, Position + new Vector3(Scale, Scale, 1f));

            return frustum.InFrustum(aabb);
        }
    }
}
