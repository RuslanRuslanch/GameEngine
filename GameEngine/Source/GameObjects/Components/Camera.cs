using GameEngine.GameObjects;
using GameEngine.Resources;
using OpenTK.Mathematics;

namespace GameEngine.Components
{
    public sealed class Camera : Component
    {
        public const float ZNear = 0.01f;
        public const float ZFar = 1000.0f;

        public Matrix4 ProjectionMatrix { get; private set; } = Matrix4.Identity;
        public Matrix4 ViewMatrix { get; private set; } = Matrix4.Identity;

        public int FOV { get; private set; } = 90;
        public Frustum Frustum { get; private set; }

        public Camera(GameObject gameObject) : base(gameObject)
        {
        }

        public override void OnStart()
        {
            base.OnStart();
            
            UpdateProjectionMatrix();
            UpdateViewMatrix();

            Frustum = new Frustum(this);
            Frustum.Initialize();
        }

        public override void OnPreRender()
        {
            
        }

        public override void OnUpdate(float delta)
        {
            UpdateViewMatrix();

            Frustum.RecalculatePlanes();
        }

        public void SetFOV(int fov)
        {
            if (fov <= 0 || fov >= 180)
            {
                throw new Exception("Invalid FOV's value");
            }

            FOV = fov;

            UpdateProjectionMatrix();
        }

        public void UpdateProjectionMatrix()
        {
            var windowSize = GameObject.World.Core.Window.ClientSize;

            var aspect = (float)windowSize.X / windowSize.Y;
            var fov = MathHelper.DegreesToRadians(FOV);

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fov, aspect, ZNear, ZFar);
        }

        public void UpdateViewMatrix()
        {
            var transform = GameObject.Transform;

            ViewMatrix = Matrix4.LookAt(transform.Position, transform.Position + transform.Forward, transform.Up);
        }
    }
}
