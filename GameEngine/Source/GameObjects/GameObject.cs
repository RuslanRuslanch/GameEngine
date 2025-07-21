using GameEngine.Components;
using GameEngine.Resources;
using GameEngine.Worlds;

namespace GameEngine.GameObjects
{
    public sealed class GameObject : IDisposable
    {
        public readonly List<Component> Components = new List<Component>();
        public readonly List<string> Tags = new List<string>();

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

            Tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            if (HasTag(tag) == false)
            {
                return;
            }

            Tags.Remove(tag);
        }

        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }

        public void OnUpdate(float delta)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].OnUpdate(delta);
            }
        }

        public void OnRender()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].OnPreRender();
                Components[i].OnRender();
            }
        }

        public void OnTick()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].OnTick();
            }
        }

        public void OnStart()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].OnStart();
            }
        }

        public void OnFinish()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].OnFinish();
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
            return (T)Components.FirstOrDefault(c => c is T);
        }

        public T AddComponent<T>() where T : Component
        {
            if (HasComponent<T>())
            {
                throw new Exception($"{this} contains {typeof(T)} component yet");
            }

            var instance = (T)Activator.CreateInstance(typeof(T), args: this);

            Components.Add(instance);

            return instance;
        }

        public void AddComponent(Component instance)
        {
            if (Components.Contains(instance))
            {
                return;
            }

            Components.Add(instance);
        }

        public void RemoveComponent<T>() where T : Component
        {
            if (TryGetComponent(out T result) == false)
            {
                throw new NullReferenceException($"{this} hasn't {typeof(T)} component");
            }

            Components.Remove(result);
        }

        public bool CanRender(Frustum frustum)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].CanRender(frustum))
                {
                    continue;
                }
                
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
