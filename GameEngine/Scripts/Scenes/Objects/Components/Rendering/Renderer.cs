namespace GameEngine.Scripts.Scenes.Objects.Components.Rendering
{
    public abstract class Renderer : Component
    {
        public override void Render() => Draw();

        public abstract void Draw();
    }
}
