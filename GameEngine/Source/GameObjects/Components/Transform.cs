using GameEngine.GameObjects;
using OpenTK.Mathematics;

namespace GameEngine.Components
{
    public sealed class Transform : Component
    {
        public Matrix4 ModelMatrix { get; private set; } = Matrix4.Identity;

        public Vector3 Position { get; private set; }
        public Vector3 Scale { get; private set; } = Vector3.One;
        public Quaternion Rotation { get; private set; } = Quaternion.Identity;

        public Vector3 Forward { get; private set; } = -Vector3.UnitZ;
        public Vector3 Up { get; private set; } = Vector3.UnitY;
        public Vector3 Right { get; private set; } = Vector3.UnitX;

        public Transform(GameObject gameObject) : base(gameObject)
        {
            
        }

        public override void OnStart()
        {
            base.OnStart();

            UpdateModelMatrix();
        }

        public void SetPosition(float x, float y, float z)
        {
            var position = new Vector3(x, y, z);

            SetPosition(position);
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;

            UpdateModelMatrix();
        }

        public void Move(Vector3 velocity)
        {
            SetPosition(Position + velocity);
        }

        public void Move(float x, float y, float z)
        {
            SetPosition(Position.X + x, Position.Y + y, Position.Z + z);
        }

        public void Rotate(Quaternion rotation)
        {
            SetRotation(Rotation + rotation);
        }

        public void SetRotation(Quaternion rotation)
        {
            Rotation = rotation;

            UpdateModelMatrix();
            RecalculateDirections();
        }

        public void Rotate(Vector3 rotation)
        {
            SetRotation(Rotation + Quaternion.FromEulerAngles(rotation));
        }

        public void SetRotation(Vector3 rotation)
        {
            SetRotation(Quaternion.FromEulerAngles(rotation));
        }

        public void SetScale(Vector3 scale)
        {
            Scale = scale;

            UpdateModelMatrix();
        }

        public void UpdateModelMatrix()
        {
            var rotation = Matrix4.CreateFromQuaternion(Rotation);
            var translation = Matrix4.CreateTranslation(Position);
            var scale = Matrix4.CreateScale(Scale);

            ModelMatrix =
                scale *
                rotation *
                translation;
        }

        private void RecalculateDirections() 
        {
            var cosX = (float)Math.Cos(Rotation.X);
            var sinX = (float)Math.Sin(Rotation.X);
            var cosY = (float)Math.Cos(Rotation.Y);
            var sinY = (float)Math.Sin(Rotation.Y);

            var forward = new Vector3(
                sinY * cosX,
                sinX,
                -cosY * cosX
            );

            Forward = Vector3.Normalize(forward);
            Right = Vector3.Normalize(Vector3.Cross(Forward, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Forward));
        }
    }
}
