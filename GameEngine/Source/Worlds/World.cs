using GameEngine.Components;
using GameEngine.GameObjects;
using GameEngine.Resources;
using OpenTK.Mathematics;

namespace GameEngine.Worlds
{
    public sealed class World
    {
        private readonly HashSet<GameObject> _gameObjects = new HashSet<GameObject>();
        
        public Camera MainCamera { get; private set; }

        public void Register(GameObject gameObject)
        {
            if (_gameObjects.Contains(gameObject))
            {
                return;
            }

            if (gameObject.HasTag("Main Camera"))
            {
                MainCamera = gameObject.GetComponent<Camera>();
            }

            gameObject.OnStart();

            _gameObjects.Add(gameObject);
        }

        public void Unregister(GameObject gameObject)
        {
            gameObject.OnFinish();

            _gameObjects.Remove(gameObject);
        }

        public void UnregisterAll()
        {
            foreach (var gameObject in _gameObjects)
            {
                Unregister(gameObject);
            }
        }

        public T FindByType<T>() where T : Component
        {
            foreach (var gameObject in _gameObjects)
            {
                if (gameObject.TryGetComponent(out T result))
                {
                    return result;
                }
            }

            return null;
        }

        public GameObject FindByTag(string tag)
        {
            foreach (var gameObject in _gameObjects)
            {
                if (gameObject.HasTag(tag))
                {
                    return gameObject;
                }
            }

            return null;
        }

        public void OnUpdate(float delta)
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.OnUpdate(delta);
            }
        }

        public void OnRender()
        {
            foreach (var gameObject in _gameObjects)
            {
                if (gameObject.CanRender(MainCamera.Frustum))
                {
                    gameObject.OnRender();
                }
            }
        }

        public void OnTick()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.OnTick();
            }
        }

        public void OnStart()
        {
            RegisterCameraPrefab();

            var camera = Resources.Resources.Get<Prefab>("cameraPrefab").GameObject;

            var gameObject = new GameObject(this);

            gameObject.Transform.Move(0f, 0f, -1f);

            var renderer = gameObject.AddComponent<LineRenderer>();
            var source = gameObject.AddComponent<SoundSource>();

            renderer.SetPoints(new[] { Vector3.Zero, Vector3.One });

            Register(gameObject);
            Register(camera);
        }

        private void RegisterCameraPrefab()
        {
            var camera = new GameObject(this);

            camera.AddTag("Main Camera");
            camera.AddComponent<Camera>();
            camera.AddComponent<CameraMovement>();

            Resources.Resources.Save(new Prefab("cameraPrefab", camera));
        }

        public void OnFinish()
        {
            UnregisterAll();
        }
    }
}
