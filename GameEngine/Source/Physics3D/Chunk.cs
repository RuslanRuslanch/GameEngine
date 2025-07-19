using GameEngine.Components;
using GameEngine.Resources;
using OpenTK.Mathematics;

namespace GameEngine.Physics3D
{
    public sealed class Chunk
    {
        private readonly Vector3 Position;
        private readonly Vector3 Size;

        private readonly AABB _aabb;
        private readonly HashSet<Collider> _colliders = new HashSet<Collider>();

        public Chunk(Vector3 position, Vector3 size)
        {
            Position = position;
            Size = size;

            _aabb = new AABB(Position, Position + Size);
        }

        public void Register(Collider collider)
        {
            if (_colliders.Contains(collider))
            {
                return;
            }

            _colliders.Add(collider);
        }

        public void Unregister(Collider collider)
        {
            _colliders.Remove(collider);
        }

        public bool IsInside(Collider collider)
        {
            return collider.Intersects(_aabb);
        }
    }
}
