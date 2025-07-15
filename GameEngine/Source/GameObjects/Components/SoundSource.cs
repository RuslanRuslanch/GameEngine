using System.Diagnostics;
using System.Numerics;
using GameEngine.GameObjects;
using GameEngine.Resources;
using OpenTK.Audio.OpenAL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GameEngine.Components
{
    public sealed class SoundSource : Component
    {
        private Sound _sound;
        private int _id;

        public bool IsPlaying { get; private set; }
        public bool IsLoop { get; private set; } = false;

        public SoundSource(GameObject gameObject) : base(gameObject)
        {
        }

        public override unsafe void OnStart()
        {
            _sound = new Sound(@"Sounds\Sound.wav");

            _id = AL.GenSource();

            AL.Source(_id, ALSourcef.Pitch, 1.0f);
            AL.Source(_id, ALSourcef.Gain, 1.0f);
            AL.Source(_id, ALSourceb.Looping, IsLoop);
        }

        public override void OnFinish()
        {
            Stop();

            AL.DeleteSource(_id);
        }

        public override void OnUpdate(float delta)
        {
            var position = GameObject.Transform.Position;

            AL.Source(_id, ALSource3f.Position, position.X, position.Y, position.Z);

            if (GameObject.World.Core.Input.Keyboard.IsKeyReleased(Keys.O))
            {
                SetLoop(true);

                Stop();
                Play(_sound);
            }
        }

        public void SetLoop(bool state)
        {
            IsLoop = state;

            AL.Source(_id, ALSourceb.Looping, IsLoop);
        }

        public void SetDirection(Vector3 direction)
        {
            AL.Source(_id, ALSource3f.Direction, direction.X, direction.Y, direction.Z);
        }

        public void SetMaxDistance(float distance)
        {
            if (distance < 0f)
            {
                throw new Exception($"Hear distance must be more or equal 0");
            }

            AL.Source(_id, ALSourcef.MaxDistance, distance);
            AL.Source(_id, ALSourcei.SourceType, (int)ALSourceType.Static);
        }

        public void SetSound(Sound sound)
        {
            Stop();

            _sound = sound;
        }

        public void Play(Sound sound)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            AL.Source(_id, ALSourcei.Buffer, sound.ID);
            AL.SourcePlay(_id);

            stopwatch.Stop();
            
            Console.WriteLine($"Execute time: {stopwatch.ElapsedMilliseconds}");
        }

        public void Stop()
        {
            AL.SourceStop(_id);
        }

        public void Pause()
        {
            AL.SourcePause(_id);
        }

        public void Unpause()
        {
            Play(_sound);
        }
    }
}