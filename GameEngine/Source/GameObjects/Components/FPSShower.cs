using GameEngine.GameObjects;

namespace GameEngine.Components
{
    public sealed class FPSShower : Component
    {
        private TextRenderer _renderer;
        private float _timer;

        public FPSShower(GameObject gameObject) : base(gameObject)
        {
        }

        public override void OnStart()
        {
            base.OnStart();

            _renderer = GameObject.GetComponent<TextRenderer>();
        }

        public override void OnUpdate(float delta)
        {
            _timer += delta;

            if (_timer < 1f)
            {
                return;
            }

            _timer -= 1f;

            var text = ((int)(1f / delta)).ToString();

            _renderer.SetText(text);
        }
    }
}