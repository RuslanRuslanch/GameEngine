using GameEngine.Components;
using GameEngine.Resources;
using GameEngine.Worlds;

namespace GameEngine.GameObjects
{
    public sealed class GameObject
    {
        private readonly HashSet<Component> _components = new HashSet<Component>();
        private readonly HashSet<string> _tags = new HashSet<string>();

        public readonly Transform Transform;
        
        public readonly World World;

        public GameObject(World world)
        {
            World = world;

            Transform = AddComponent<Transform>();
        }

        public void AddTag(string tag)
        {
            if (HasTag(tag))
            {
                return;
            }

            _tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            if (HasTag(tag) == false)
            {
                return;
            }

            _tags.Remove(tag);
        }

        public bool HasTag(string tag)
        {
            return _tags.Contains(tag);
        }

        public void OnUpdate(float delta)
        {
            foreach (var component in _components)
            {
                component.OnUpdate(delta);
            }
        }

        public void OnRender()
        {
            foreach (var component in _components)
            {
                component.OnPreRender();
                component.OnRender();
            }
        }

        public void OnTick()
        {
            foreach (var component in _components)
            {
                component.OnTick();
            }
        }

        public void OnStart()
        {
            foreach (var component in _components)
            {
                component.OnStart();
            }
        }

        public void OnFinish()
        {
            foreach (var component in _components)
            {
                component.OnFinish();
            }
        }

        public bool TryGetComponent<T>(out T result) where T : Component
        {
            result = GetComponent<T>();

            if (result == null)
            {
                return false;
            }

            return true;
        }

        public bool HasComponent<T>() where T : Component
        {
            return GetComponent<T>() != null;
        }

        public T GetComponent<T>() where T : Component
        {
            return (T)_components.FirstOrDefault(c => c is T);
        }

        public T AddComponent<T>() where T : Component
        {
            if (HasComponent<T>())
            {
                throw new Exception($"{this} contains {typeof(T)} component yet");
            }

            var instance = (T)Activator.CreateInstance(typeof(T), args: this);

            _components.Add(instance);

            return instance;
        }

        public void RemoveComponent<T>() where T : Component
        {
            if (TryGetComponent(out T result) == false)
            {
                throw new NullReferenceException($"{this} hasn't {typeof(T)} component");
            }

            _components.Remove(result);
        }

        public bool CanRender(Frustum frustum)
        {
            foreach (var component in _components)
            {
                if (component.CanRender(frustum))
                {
                    continue;
                }
                
                return false;
            }

            return true;
        }
    }
}
