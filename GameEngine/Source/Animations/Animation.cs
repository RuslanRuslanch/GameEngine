namespace GameEngine.Animations
{
    public sealed class Animation
    {
        public bool IsPaused { get; private set; }
        public bool IsPlaying { get; private set; }

        private readonly AnimationSheet[] _sheets;

        public readonly float Length;

        public int CurrentIndex { get; private set; }
        public AnimationSheet CurrentSheet => _sheets[CurrentIndex];

        public Animation(AnimationSheet[] sheets)
        {
            _sheets = sheets;

            foreach (var sheet in _sheets)
            {
                Length += sheet.Length;
            }
        }

        public void Play()
        {
            if (IsPlaying)
            {
                return;
            }

            IsPlaying = true;

            Update();
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public void Restart()
        {
            Stop();
            Play();
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Unpause()
        {
            IsPaused = false;
        }

        private async void Update()
        {
            while (IsPlaying)
            {
                if (IsPaused)
                {
                    continue;
                }

                CurrentSheet.ApplyChanges();

                await Task.Delay((int)(CurrentSheet.Length * 1000f));

                Next();
            }
        }

        private void Next()
        {
            if (CurrentIndex + 1 == _sheets.Length)
            {
                CurrentIndex = 0;
            }

            CurrentIndex++;
        }
    }
}