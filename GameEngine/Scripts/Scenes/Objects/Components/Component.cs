namespace GameEngine.Scripts.Scenes.Objects.Components
{
    public abstract class Component
    {
        public virtual void Load() { }
        public virtual void Unload() { }
        public virtual void Update() { }
        public virtual void Render() { }
    }
}
