namespace GameEngine.Core
{
    public sealed class Timer
    {
        public event Action TimeOut;
        public event Action Started;
        public event Action Stoped;

        public async Task Start(float time)
        {
            Started?.Invoke();

            await Task.Delay((int)(time * 1000f));

            TimeOut?.Invoke();
            Stoped?.Invoke();
        }

        public async Task StartLoop(float interval, int iterations)
        {
            Started?.Invoke();

            for (int i = 0; i < iterations; i++)
            {
                await Task.Delay((int)(interval * 1000f));

                TimeOut?.Invoke();
            }

            Stoped?.Invoke();
        }

        public async Task StartLoop(float interval, Func<bool> condition)
        {
            Started?.Invoke();

            while (condition.Invoke())
            {
                await Task.Delay((int)(interval * 1000f));

                TimeOut?.Invoke();
            }

            Stoped?.Invoke();
        }
    }
}
