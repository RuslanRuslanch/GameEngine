using GameEngine.GameObjects;
using OpenTK.Audio.OpenAL;

namespace GameEngine.Components
{
    public sealed class AudioListener : Component
    {
        public readonly int ID;

        public AudioListener(GameObject gameObject) : base(gameObject)
        {
        }

        public override void OnUpdate(float delta)
        {
            var position = GameObject.Transform.Position;

            AL.Listener(ALListener3f.Position, position.X, position.Y, position.Z);
        }
    }
}