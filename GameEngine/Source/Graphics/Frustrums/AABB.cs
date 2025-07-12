using OpenTK.Mathematics;

namespace GameEngine.Graphics
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
    }
}

