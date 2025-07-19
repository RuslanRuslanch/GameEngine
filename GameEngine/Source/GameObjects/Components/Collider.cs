using GameEngine.GameObjects;
using GameEngine.Resources;
using OpenTK.Mathematics;

namespace GameEngine.Components
{
    public sealed class Collider : Component
    {
        public readonly AABB AABB;

        public Vector3 WorldSpaceMin => AABB.Min + GameObject.Transform.Position;
        public Vector3 WorldSpaceMax => AABB.Max + GameObject.Transform.Position;

        public Collider(Vector3 min, Vector3 max, GameObject gameObject) : base(gameObject)
        {
            AABB = new AABB(min, max);
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

        public bool Intersects(AABB another)
        {
            return (WorldSpaceMin.X <= another.Max.X && WorldSpaceMax.X >= another.Min.X) &&
                   (WorldSpaceMin.Y <= another.Max.Y && WorldSpaceMax.Y >= another.Min.Y) &&
                   (WorldSpaceMin.Z <= another.Max.Z && WorldSpaceMax.Z >= another.Min.Z);
        }
    }
}
