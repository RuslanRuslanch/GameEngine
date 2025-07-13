using GameEngine.GameObjects;
using GameEngine.Inputs;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Components
{
    public sealed class CameraMovement : Component
    {
        public const float Speed = 10f;

        public CameraMovement(GameObject gameObject) : base(gameObject)
        {

        }

        public override void OnUpdate(float delta)
        {
            var input = GameObject.World.Core.Input.Keyboard;
            var direction = Vector3.Zero;

            if (input.IsKeyDown(Keys.W))
            {
                direction += GameObject.Transform.Forward;
            }
            if (input.IsKeyDown(Keys.S))
            {
                direction -= GameObject.Transform.Forward;
            }

            if (input.IsKeyDown(Keys.D))
            {
                direction += GameObject.Transform.Right;
            }
            if (input.IsKeyDown(Keys.A))
            {
                direction -= GameObject.Transform.Right;
            }

            if (input.IsKeyDown(Keys.Space))
            {
                direction += GameObject.Transform.Up;
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                direction -= GameObject.Transform.Up;
            }

            GameObject.Transform.Move(direction * (delta * Speed));
        }
    }
}
