using GameEngine.GameObjects;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Animations
{
    public struct AnimationSheet
    {
        public readonly GameObject GameObject;
        public readonly float Length;

        public readonly Delegate ApplyChangesMethod;
        public readonly object[] Args;

        public AnimationSheet(Delegate method, params object[] args)
        {
            ApplyChangesMethod = method;
            Args = args;
        }

        public void ApplyChanges()
        {
            ApplyChangesMethod?.DynamicInvoke(Args);
        }
    }
}