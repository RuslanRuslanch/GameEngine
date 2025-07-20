using GameEngine.Debugs;
using GameEngine.Resources;
using GameEngine.Inputs;
using GameEngine.Physics3D;
using GameEngine.FileSystems;
using GameEngine.Graphics;
using GameEngine.Sounds;

namespace GameEngine
{
    public sealed class Core
    {
        public readonly Gizmos Gizmos = new Gizmos();
        public readonly Physics Physics = new Physics();
        public readonly Resource Resource = new Resource();
        public readonly RenderSystem Render = new RenderSystem();
        public readonly SoundSystem SoundSystem = new SoundSystem();
        public readonly FileSystem FileSystem;
        public readonly RenderWindow Window;
        public readonly Input Input;

        public Core(RenderWindow window)
        {
            Window = window;

            FileSystem = new FileSystem(Resource);
            Input = new Input(window);
        }
    }
}
