using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GameEngine.Scripts.Scenes.Objects.Components
{
    public sealed class Transform : Component
    {
        public Matrix4 Model { get; private set; } 

        public Vector3 Position { get; private set; }
        public Vector3 Scale { get; private set; }
        public Vector3 Rotation { get; private set; }

        public event Action TransformChanged;

        public Transform(Vector3 position, Vector3 scale, Vector3 rotation)
        {
            SetPosition(position);
            SetRotation(rotation);
            SetScale(scale);

            Recalculate();

            TransformChanged += Recalculate;
        }

        public void Move(Vector3 velocity)
        {
            if (velocity == Vector3.Zero)
            {
                return;
            }

            Position += velocity;

            TransformChanged?.Invoke();
        }

        public void SetPosition(Vector3 position)
        {
            if (Position == position)
            {
                return;
            }

            Position = position;

            TransformChanged?.Invoke();
        }

        public void SetScale(Vector3 scale)
        {
            if (Scale == scale)
            {
                return;
            }
            if (scale.X < 0f || scale.Y < 0f || scale.Z < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(scale));
            }

            Scale = scale;

            TransformChanged?.Invoke();
        }

        public void Rotate(Vector3 rotation)
        {
            if (rotation == Vector3.Zero)
            {
                return;
            } 

            Rotation += rotation;

            TransformChanged?.Invoke();
        }

        public void SetRotation(Vector3 rotation)
        {
            if (Rotation == rotation)
            {
                return;
            }

            Rotation = rotation;

            TransformChanged?.Invoke();
        }


        public void Recalculate()
        {
            GL.PushMatrix();

            float factor = 57.29578f;

            GL.Translate(Position.X, Position.Y, Position.Z);
            GL.Rotate(Rotation.Z * factor, 0.0f, 0.0f, 1.0f);
            GL.Rotate(Rotation.Z * factor, 0.0f, 1.0f, 0.0f);
            GL.Rotate(Rotation.Z * factor, 1.0f, 0.0f, 0.0f);
            GL.Scale(Scale);

            GL.PopMatrix();

            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();
            //
            //float angleX = MathHelper.DegreesToRadians(Rotation.X);
            //float angleY = MathHelper.DegreesToRadians(Rotation.Y);
            //float angleZ = MathHelper.DegreesToRadians(Rotation.Z);
            //
            //Matrix4 model =
            //    Matrix4.CreateTranslation(Position) *
            //   Matrix4.CreateRotationX(angleX) *
            //    Matrix4.CreateRotationY(angleY) *
            //    Matrix4.CreateRotationZ(angleZ) *
            //    Matrix4.CreateScale(Scale);
            //
            //GL.LoadMatrix(ref model);
            //
            //Model = model;
        }
    }
}
