using GameEngine.GameObjects;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Components
{
    public sealed class CharacterMovement : Component
    {
        public const float Speed = 5f;

        public CharacterMovement(GameObject gameObject) : base(gameObject)
        {
        }

        public override void OnUpdate(float delta)
        {
            var keyboard = GameObject.World.Core.Input.Keyboard;
            var direction = Vector3.Zero;

            if (keyboard.IsKeyDown(Keys.W))
            {
                direction += GameObject.Transform.Forward;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                direction -= GameObject.Transform.Forward;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                direction += GameObject.Transform.Right;
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                direction -= GameObject.Transform.Right;
            }

            GameObject.Transform.Move(direction * (delta * Speed));
        }
    }
}