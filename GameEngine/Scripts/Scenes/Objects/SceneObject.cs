using GameEngine.Scripts.Scenes.Objects.Components;

namespace GameEngine.Scripts.Scenes.Objects
{
    public abstract class SceneObject
    {
        private readonly List<Component> _components = new List<Component>();

        public void AddComponent(Component component)
        {
            _components.Add(component);
        }
        public void RemoveComponent(Component component)
        {
            if (_components.Contains(component) == false)
            {
                throw new NullReferenceException(nameof(component));
            }

            _components.Remove(component);
        }
        public Component GetComponent<T>() where T : Component
        {
            foreach (var component in _components)
            {
                if (component.GetType() == typeof(T))
                {
                    return component;
                }
            }

            return null;
        }

        public virtual void Load()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Load();
            }
        }

        public virtual void Unload()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Unload();
            }
        }

        public virtual void Update()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Update();
            }
        }

        public virtual void Render()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Render();
            }
        }
    }
}
