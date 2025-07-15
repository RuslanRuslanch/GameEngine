using GameEngine.Bootstraps;
using GameEngine.Worlds;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace GameEngine.Graphics
{
    public sealed class RenderWindow : GameWindow
    {
        private readonly Core _core;
        private readonly TickSystem _tickSystem;
        private readonly ResourceLoader _resourceLoader;
        private readonly World _world;

        private float _timer;

        public RenderWindow(GameWindowSettings gameSettings, NativeWindowSettings nativeSettings) : base(gameSettings, nativeSettings)
        {
            _core = new Core(this);

            _world = new World(_core);
            _tickSystem = new TickSystem(_world);
            _resourceLoader = new ResourceLoader();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _resourceLoader.Initialize(_core.Resource);

            _world.OnStart();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            _resourceLoader.Deinitialize();

            _world.OnFinish();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.ClearColor(Color4.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);

            _world.OnRender();

            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.Texture2D);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var delta = (float)args.Time;

            _world.HandleRegisterRequests();
            
            _world.OnUpdate(delta);
            _tickSystem.OnTick(delta);

            _timer += delta;

            if (_timer >= 1f)
            {
                Title = $"FPS: {(int)(1d / delta)}";

                _timer -= 1f;
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}
