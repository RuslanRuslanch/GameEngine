using GameEngine.Resources;
using OpenTK.Mathematics;

namespace GameEngine.Components
{
    public sealed class Particle
    {
        public Matrix4 ModelMatrix { get; private set; } = Matrix4.Identity;

        public Vector3 Position { get; private set; }
        public float Scale { get; private set; } = 1f;

        public AABB AABB { get; private set; } = new AABB();

        public Particle(Vector3 position, float scale)
        {
            Position = position;
            Scale = scale;

            ReloadModelMatrix();
        }

        public void Move(float x, float y, float z)
        {
            var velocity = new Vector3(x, y, z);

            Move(velocity);
        }

        public void Move(Vector3 velocity)
        {
            Position += velocity;

            var scale = new Vector3(Scale, Scale, 1f);

            AABB = new AABB(Position, Position + scale);

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
    }
}
