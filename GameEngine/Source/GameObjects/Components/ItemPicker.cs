using GameEngine.GameObjects;
using OpenTK.Mathematics;

namespace GameEngine.Components
{
    public sealed class ItemPicker : Component
    {
        private Timer _timer;

        public ItemPicker(GameObject gameObject) : base(gameObject)
        {
        }

        public override void OnStart()
        {
            base.OnStart();

            _timer = GameObject.World.FindByType<Timer>();
        }

        public override void OnUpdate(float delta)
        {
            var targets = GameObject.World.FindObjectsByTag("Pickable");

            foreach (var target in targets)
            {
                var distance = Vector3.Distance(GameObject.Transform.Position, target.Transform.Position);

                if (distance > 1f)
                {
                    continue;
                }

                GameObject.World.Unregister(target);

                Console.WriteLine("Player pick item");

                _timer.AddTime(1);
                target.Dispose();
            }
        }
    }
}