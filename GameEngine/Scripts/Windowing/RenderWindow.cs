using GameEngine.Scripts.Scenes;
using GameEngine.Scripts.Windowing.Debuging;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace GameEngine.Scripts.Windowing
{
    public sealed class RenderWindow : GameWindow
    {
        private readonly bool _usePrepass = false;

        private readonly FPSCounter _counter;
        private readonly Scene _scene;

        public RenderWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            _counter = new FPSCounter();
            _scene = new Scene();

            Time time = new Time(this);
            Input input = new Input(KeyboardState, MouseState);
            FPSCounterView counterView = new FPSCounterView(_counter, this);

            CursorState = CursorState.Grabbed;
        }

        protected override void OnResize(ResizeEventArgs resizeEvent)
        {
            base.OnResize(resizeEvent);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            //GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);

            _scene.Load();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);

            _scene.Unload();
        }

        protected override void OnRenderFrame(FrameEventArgs frameEvent)
        {
            base.OnRenderFrame(frameEvent);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_usePrepass)
            {
                GL.Disable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Less);
                GL.ColorMask(false, false, false, false);
                GL.DepthMask(true);

                _scene.Render();

                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Lequal);
                GL.ColorMask(true, true, true, true);
                GL.DepthMask(false);
            }

            _scene.Render();

            _counter.Calculate();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs frameEvent)
        {
            base.OnUpdateFrame(frameEvent);

            _scene.Update();
        }
    }
}
