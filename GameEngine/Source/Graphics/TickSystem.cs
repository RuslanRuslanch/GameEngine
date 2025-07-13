using GameEngine.Worlds;

namespace GameEngine.Resources
{
    public sealed class TickSystem
    {
        public const int TPS = 30;
        public const float Delta = 1f / TPS;

        private readonly World _world;

        private float _timer;

        public TickSystem(World world)
        {
            _world = world;
        }

        public void OnTick(float delta)
        {
            _timer += delta;

            if (_timer < Delta)
            {
                return;
            }

            _world.OnTick();

            _timer -= Delta;
        }
    }
}
