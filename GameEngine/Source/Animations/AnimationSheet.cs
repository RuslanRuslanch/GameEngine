using GameEngine.GameObjects;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Animations
{
    public struct AnimationSheet
    {
        public readonly GameObject GameObject;
        public readonly float Length;

        public readonly Delegate ApplyChangesMethod;

        public AnimationSheet(GameObject gameObject, float length, Delegate method)
        {
            GameObject = gameObject;
            Length = length;
            ApplyChangesMethod = method;
        }

        public void ApplyChanges()
        {
            ApplyChangesMethod?.DynamicInvoke();
        }
    }
}