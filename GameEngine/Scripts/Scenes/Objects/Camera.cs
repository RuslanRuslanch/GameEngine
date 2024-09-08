namespace GameEngine.Scripts.Scenes.Objects
{
    public sealed class Camera : SceneObject
    {
        public const float ScreenAspect = 16f / 10f;

        public const float FOV = 90f;

        public const float ZFar = 1000f;
        public const float ZNear = 0.001f;

        public const int ViewRadius = 8;
    }
}
