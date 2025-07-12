using GameEngine.Components;
using OpenTK.Mathematics;

namespace GameEngine.Physics3D
{
    public struct RaycastHit
    {
        public readonly Collider Collider;
        public readonly Vector3 Point;
        public readonly float Distance;

        public RaycastHit(Collider collider, Vector3 point, float distance)
        {
            Collider = collider;
            Point = point;
            Distance = distance;
        }
    }
}
