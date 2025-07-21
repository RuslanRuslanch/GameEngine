using GameEngine.GameObjects;

namespace GameEngine.Components
{
    public sealed class Timer : Component
    {
        private float _delayTimer;
        private int _time;

        private TextRenderer _renderer;

        public Timer(GameObject gameObject) : base(gameObject)
        {
        }

        public override void OnStart()
        {
            base.OnStart();

            _renderer = GameObject.GetComponent<TextRenderer>();
        }

        public override void OnUpdate(float delta)
        {
            if (_time == 0)
            {
                GameObject.World.Core.Window.Close();
            }

            _delayTimer += delta;

            if (_delayTimer < 0.33f)
            {
                return;
            }

            _delayTimer -= 1f;

            TakeTime();
        }

        public void SetTime(int time)
        {
            _time = time;
        }

        public void TakeTime()
        {
            _time -= 1;

            _renderer.SetText(_time.ToString());
        }

        public void AddTime(int t)
        {
            _time += t;

            _renderer.SetText(_time.ToString());
        }
    }
}