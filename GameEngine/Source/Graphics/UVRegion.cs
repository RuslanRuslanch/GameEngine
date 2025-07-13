using OpenTK.Mathematics;

namespace GameEngine.Resources
{
    public struct UVRegion
    {
        public readonly Vector2 Min;
        public readonly Vector2 XOffset;
        public readonly Vector2 YOffset;
        public readonly Vector2 Max;

        public UVRegion()
        {
            Min = Vector2.Zero;
            XOffset = Vector2.UnitX;
            YOffset = Vector2.UnitY;
            Max = Vector2.One;
        }

        public UVRegion(Vector2 min, Vector2 xOffset, Vector2 yOffset, Vector2 max)
        {
            Min = min;
            XOffset = xOffset;
            YOffset = yOffset;
            Max = max;
        }
    }
}
