using System.Diagnostics;
using GameEngine.GameObjects;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Components
{
    public sealed class ObjectSpawner : Component
    {
        private int _count;

        public ObjectSpawner(GameObject gameObject) : base(gameObject)
        {
        }

        public override void OnUpdate(float delta)
        {
            var keyboard = GameObject.World.Core.Input.Keyboard;

            if (keyboard.IsKeyReleased(Keys.E))
            {
                var stopwatch = Stopwatch.StartNew();   

                for (int x = 0; x < 100; x++)
                {
                    for (int z = 0; z < 100; z++)
                    {
                        var gameObject = new GameObject(GameObject.World);

                        gameObject.AddComponent<SpriteRenderer>();
                        gameObject.Transform.SetPosition(new Vector3(-x, 0f, -z));

                        GameObject.World.SendRegisterRequest(gameObject);

                        _count++;
                    }
                }

                stopwatch.Stop();

                Console.WriteLine($"Spawn time: {stopwatch.ElapsedMilliseconds} | {_count} units");
            }
        }
    }
}