using System.Diagnostics;
using GameEngine.GameObjects;
using GameEngine.Graphics;
using GameEngine.Resources;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Components
{
    public sealed class ParticleRenderer : AbstractRenderer
    {
        public bool IsPlaying { get; private set; } = false;
        public int MaxParticleCount { get; private set; } = 0;

        private readonly List<Particle> _particles = new List<Particle>();
        private readonly UVRegion _uvRegion = new UVRegion();
        private readonly FastNoiseLite _noise = new FastNoiseLite();

        private int _vao;
        private int _vertexObject;
        private int _uvObject;
        private int _normalObject;
        private int _ebo;

        private float _time;

        public ParticleRenderer(GameObject gameObject) : base(gameObject)
        {
        }

        public override void OnStart()
        {
            base.OnStart();

            var material = GameObject.World.Core.Resource.Get<Material>("ParticleMaterial");

            SetMaterial(material);
            CreateBuffers();
        }

        public override void OnUpdate(float delta)
        {
            if (GameObject.World.Core.Input.Keyboard.IsKeyPressed(Keys.P))
            {
                Stop();
                SetMaxParticleCount(MaxParticleCount + 1000);
                Play();

                Console.WriteLine($"Current particle count: {MaxParticleCount}");
            }

            if (IsPlaying == false)
            {
                return;
            }

            _time += delta;

            for (int i = 0; i < MaxParticleCount; i++)
            {
                var particlePosition = _particles[i].Position;

                var x = (float)Math.Sin(_time * particlePosition.X + i);
                var y = 1f;
                var z = (float)Math.Cos(_time * particlePosition.Z + i);

                var direction = new Vector3(x, y, z);

                _particles[i].Move(direction * delta);
            }
        }

        public override void OnPreRender()
        {
            if (IsPlaying == false)
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
            if (IsPlaying == false)
            {
                return;
            }

            GameObject.World.Core.Render.Bind(Material);

            GL.BindVertexArray(_vao);

            RenderParticles();

            GL.BindVertexArray(0);
        }

        private void RenderParticles()
        {
            var frustum = GameObject.World.MainCamera.Frustum;

            for (int i = 0; i < MaxParticleCount; i++)
            {
                if (frustum.InFrustum(_particles[i].AABB) == false)
                {
                    continue;
                }

                var model = _particles[i].ModelMatrix;

                Material.Shader.Load("model", ref model);

                GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }
        }


        public void Play()
        {
            if (IsPlaying)
            {
                return;
            }

            IsPlaying = true;

            if (_particles.Count == MaxParticleCount)
            {
                return;
            }

            _particles.Clear();
            
            for (int i = 0; i < MaxParticleCount; i++)
            {
                var particle = new Particle(GameObject.Transform.Position, GameObject.Transform.Scale.X);

                _particles.Add(particle);
            }
        }

        public void Stop()
        {
            _particles.Clear();

            IsPlaying = false;
        }

        public void Restart()
        {
            Stop();
            Play();
        }

        public void SetMaxParticleCount(int count)
        {
            MaxParticleCount = count;
        }

        private void DeleteBuffers()
        {
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(_vao);
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(_ebo);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexObject);
            GL.DeleteBuffer(_uvObject);
            GL.DeleteBuffer(_normalObject);
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
    }
}
