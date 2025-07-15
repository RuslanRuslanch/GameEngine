using GameEngine.Debugs;
using GameEngine.Resources;
using GameEngine.Inputs;
using GameEngine.Physics3D;
using GameEngine.FileSystems;
using GameEngine.Graphics;

namespace GameEngine
{
    public sealed class Core
    {
        public readonly Gizmos Gizmos = new Gizmos();
        public readonly Physics Physics = new Physics();
        public readonly Resource Resource = new Resource();
        public readonly FileSystem FileSystem = new FileSystem();
        public readonly RenderSystem Render = new RenderSystem();
        public readonly RenderWindow Window;
        public readonly Input Input;

        public Core(RenderWindow window)
        {
            Window = window;

            Input = new Input(window.KeyboardState, window.MouseState);
        }
    }
}
