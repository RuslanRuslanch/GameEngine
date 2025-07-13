using OpenTK.Mathematics;

namespace GameEngine.Resources
{
    public struct FrustumPlane
    {
        public readonly Vector3 Normal;
        public readonly float Distance;

        public FrustumPlane(Vector3 normal, float distance)
        {
            Normal = normal;
            Distance = distance;
        }

        public FrustumPlane(float x, float y, float z, float distance)
        {
            Normal = new Vector3(x, y, z);
            Distance = distance;
        }
    }
}

