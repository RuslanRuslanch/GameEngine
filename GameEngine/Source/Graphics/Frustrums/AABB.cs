using OpenTK.Mathematics;

namespace GameEngine.Resources
{
    public struct AABB
    {
        public readonly Vector3 Min;
        public readonly Vector3 Max;

        public AABB(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public bool Intersects(Vector3 point)
        {
            return (point.X >= Min.X && point.X <= Max.X) &&
                   (point.Y >= Min.Y && point.Y <= Max.Y) &&
                   (point.Z >= Min.Z && point.Z <= Max.Z);
        }

        public bool Intersects(AABB another)
        {
            return (Min.X <= another.Max.X && Max.X >= another.Min.X) &&
                   (Min.Y <= another.Max.Y && Max.Y >= another.Min.Y) &&
                   (Min.Z <= another.Max.Z && Max.Z >= another.Min.Z);
        }
    }
}

