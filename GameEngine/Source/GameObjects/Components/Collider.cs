using GameEngine.GameObjects;
using OpenTK.Mathematics;

namespace GameEngine.Components
{
    public sealed class Collider : Component
    {
        public readonly Vector3 Min;
        public readonly Vector3 Max;

        public Vector3 WorldSpaceMin => Min + GameObject.Transform.Position;
        public Vector3 WorldSpaceMax => Max + GameObject.Transform.Position;

        public Collider(Vector3 min, Vector3 max, GameObject gameObject) : base(gameObject)
        {
            Min = min;
            Max = max;
        }

        public bool Intersects(Vector3 point)
        {
            return (point.X >= WorldSpaceMin.X && point.X <= WorldSpaceMax.X) &&
                   (point.Y >= WorldSpaceMin.Y && point.Y <= WorldSpaceMax.Y) &&
                   (point.Z >= WorldSpaceMin.Z && point.Z <= WorldSpaceMax.Z);
        }

        public bool Intersects(Collider another)
        {
            return (WorldSpaceMin.X <= another.WorldSpaceMax.X && WorldSpaceMax.X >= another.WorldSpaceMin.X) &&
                   (WorldSpaceMin.Y <= another.WorldSpaceMax.Y && WorldSpaceMax.Y >= another.WorldSpaceMin.Y) &&
                   (WorldSpaceMin.Z <= another.WorldSpaceMax.Z && WorldSpaceMax.Z >= another.WorldSpaceMin.Z);
        }
    }
}
