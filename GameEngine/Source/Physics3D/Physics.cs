using GameEngine.Components;
using OpenTK.Mathematics;

namespace GameEngine.Physics3D
{
    public sealed class Physics
    {
        public const float StepLength = 0.1f;

        private readonly HashSet<Collider> _colliders = new HashSet<Collider>();

        public bool Intersects(Vector3 point, out Collider result)
        {
            foreach (var collider in _colliders)
            {
                if (collider.Intersects(point) == false)
                {
                    continue;
                }

                result = collider;

                return true;
            }

            result = null;

            return false;
        }

        public bool Intersects(Collider another)
        {
            foreach (var collider in _colliders)
            {
                if (collider.Intersects(another) == false)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        public Chunk[] GetChunks()
        {
            return null;
        }

        public bool Raycast(Ray ray, out RaycastHit hit, float maxDistance)
        {
            var rayOffset = ray.Direction * StepLength;
            var rayPosition = ray.Origin;

            for (float i = 0; i < maxDistance; i += StepLength)
            {
                if (Intersects(rayPosition, out var collider))
                {
                    var distance = Vector3.Distance(ray.Origin, rayPosition);

                    hit = new RaycastHit(collider, rayPosition, distance);

                    return true;
                }

                rayPosition += rayOffset;
            }

            hit = new RaycastHit();

            return false;
        }
    }
}
