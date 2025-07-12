using GameEngine.Components;
using OpenTK.Mathematics;

namespace GameEngine.Graphics
{
    public sealed class Frustum
    {
        public const int FrustumPlaneCount = 6;

        private readonly FrustumPlane[] _planes = new FrustumPlane[FrustumPlaneCount];
        private readonly Camera _camera;

        public Frustum(Camera camera)
        {
            _camera = camera;
        }

        public void Initialize()
        {
            RecalculatePlanes();
        }

        public void RecalculatePlanes()
        {
            var m = _camera.ViewMatrix * _camera.ProjectionMatrix;

            _planes[0] = new FrustumPlane(
                m.M14 + m.M11,
                m.M24 + m.M21,
                m.M34 + m.M31,
                m.M44 + m.M41);

            _planes[1] = new FrustumPlane(
                m.M14 - m.M11,
                m.M24 - m.M21,
                m.M34 - m.M31,
                m.M44 - m.M41);

            _planes[2] = new FrustumPlane(
                m.M14 + m.M12,
                m.M24 + m.M22,
                m.M34 + m.M32,
                m.M44 + m.M42);

            _planes[3] = new FrustumPlane(
                m.M14 - m.M12,
                m.M24 - m.M22,
                m.M34 - m.M32,
                m.M44 - m.M42);

            _planes[4] = new FrustumPlane(
                m.M14 + m.M13,
                m.M24 + m.M23,
                m.M34 + m.M33,
                m.M44 + m.M43);

            _planes[5] = new FrustumPlane(
                m.M14 - m.M13,
                m.M24 - m.M23,
                m.M34 - m.M33,
                m.M44 - m.M43);


            for (int i = 0; i < FrustumPlaneCount; i++)
            {
                _planes[i] = Normalize(_planes[i]);
            }
        }

        private FrustumPlane Normalize(FrustumPlane p)
        {
            var length = (float)Math.Sqrt(p.Normal.X * p.Normal.X +
                                            p.Normal.Y * p.Normal.Y +
                                            p.Normal.Z * p.Normal.Z);

            return new FrustumPlane(p.Normal / length, p.Distance / length);
        }

        public bool InFrustum(AABB aabb)
        {
            foreach (var plane in _planes)
            {
                var positiveVertex = new Vector3(
                    plane.Normal.X >= 0f ? aabb.Max.X : aabb.Min.X,
                    plane.Normal.Y >= 0f ? aabb.Max.Y : aabb.Min.Y,
                    plane.Normal.Z >= 0f ? aabb.Max.Z : aabb.Min.Z);

                var distance = Vector3.Dot(plane.Normal, positiveVertex) + plane.Distance;

                if (distance < 0f)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

