using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Scripts.Scenes.Objects.Components.Cameries
{
    public sealed class CameraMovement : Component
    {
        private readonly Transform _transform;

        private readonly float _sensivity;
        private readonly float _moveSpeed;

        private Vector3 _front = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _right = Vector3.UnitX;

        public event Action CameraRotated;

        public CameraMovement(float moveSpeed, float sensivity, Transform transform)
        {
            _transform = transform;

            _moveSpeed = moveSpeed;
            _sensivity = sensivity;

            LoadViewMatrix();
            LoadPerspectiveMatrix();
            RecalculateVectors();
        }

        public override void Update()
        {
            Move();
            Rotate();
        }

        public void Move()
        {
            Vector3 direction = Vector3.Zero;

            if (Input.IsKeyDown(Keys.W))
            {
                direction += _front; 
            }
            if (Input.IsKeyDown(Keys.S))
            {
                direction -= _front;
            }
            if (Input.IsKeyDown(Keys.A))
            {
                direction -= _right;
            }
            if (Input.IsKeyDown(Keys.D))
            {
                direction += _right;
            }
            if (Input.IsKeyDown(Keys.Space))
            {
                direction += _up;
            }
            if (Input.IsKeyDown(Keys.LeftShift))
            {
                direction -= _up;
            }

            if (direction == Vector3.Zero)
            {
                return;
            }

            direction *= _moveSpeed * (float)Time.UpdateTime;

            _transform.Move(direction);

            LoadViewMatrix();
        }

        public void Rotate()
        {
            Vector2 mouseDelta = Input.MouseDelta;

            if (mouseDelta == Vector2.Zero)
            {
                return;
            }

            mouseDelta *= _sensivity;

            float rotationY = Math.Clamp(_transform.Rotation.Y - mouseDelta.Y, -89.9f, 89.9f);
            float rotationX = _transform.Rotation.X + mouseDelta.X;

            Vector3 rotation = new Vector3(rotationX, rotationY, 0f);

            _transform.SetRotation(rotation);

            RecalculateVectors();
            LoadViewMatrix();
        }

        private unsafe void LoadViewMatrix()
        {
            var view = Matrix4.LookAt(_transform.Position, _transform.Position + _front, _up);

            GL.MatrixMode(MatrixMode.Modelview);

            GL.LoadIdentity();
            GL.LoadMatrix(ref view);
        }

        private unsafe void LoadPerspectiveMatrix()
        {
            GL.MatrixMode(MatrixMode.Projection);

            var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Camera.FOV), Camera.ScreenAspect, Camera.ZNear, Camera.ZFar);

            GL.LoadIdentity();
            GL.LoadMatrix(ref projection);
        }

        private unsafe void RecalculateVectors()
        {
            _front.X = (float)(Math.Cos(MathHelper.DegreesToRadians(_transform.Rotation.Y)) * Math.Cos(MathHelper.DegreesToRadians(_transform.Rotation.X)));
            _front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(_transform.Rotation.Y));
            _front.Z = (float)(Math.Cos(MathHelper.DegreesToRadians(_transform.Rotation.Y)) * Math.Sin(MathHelper.DegreesToRadians(_transform.Rotation.X)));

            _front = Vector3.Normalize(_front);
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }
    }
}
