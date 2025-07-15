using GameEngine.GameObjects;
using GameEngine.Resources;

namespace GameEngine.Components
{
    public abstract class Component
    {
        public bool IsInitialized { get; private set; }

        public readonly GameObject GameObject;

        public Component(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public void SetInitialized()
        {
            IsInitialized = true;
        }

        public virtual void OnUpdate(float delta) { }
        public virtual void OnPreRender() { }
        public virtual void OnRender() { }
        public virtual void OnTick() { }
        public virtual void OnStart() => SetInitialized();
        public virtual void OnFinish() { }
        public virtual bool CanRender(Frustum frustum) => true;
    }
}
