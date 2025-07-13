using System.Diagnostics;
using GameEngine.Components;
using GameEngine.GameObjects;
using GameEngine.Resources;
using OpenTK.Mathematics;

namespace GameEngine.Worlds
{
    public sealed class World
    {
        private readonly HashSet<GameObject> _gameObjects = new HashSet<GameObject>();
        
        public readonly Core Core;

        public Camera MainCamera { get; private set; }

        public World(Core core)
        {
            Core = core;
        }

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

            var camera = Core.Resource.Get<Prefab>("cameraPrefab").GameObject;
            var grid = SpawnGrid();

            Register(grid);
            Register(camera);
        }

        private GameObject SpawnGrid()
        {
            var gameObject = new GameObject(this);

            gameObject.Transform.Move(0f, 0f, -1f);

            var renderer = gameObject.AddComponent<LineRenderer>();
            var source = gameObject.AddComponent<SoundSource>();

            renderer.SetColor(Color4.GhostWhite);

            var stopwatch = Stopwatch.StartNew();

            for (int x = -32; x < 32; x++)
            {
                for (int z = -32; z < 32; z++)
                {
                    var points = new Vector3[]
                    {
                        new Vector3(x, 0f, z),
                        new Vector3(x + 1f, 0f, z),

                        new Vector3(x + 1f, 0f, z),
                        new Vector3(x + 1f, 0f, z + 1f),

                        new Vector3(x + 1f, 0f, z + 1f),
                        new Vector3(x, 0f, z),

                        new Vector3(x, 0f, z),
                        new Vector3(x, 0f, z + 1f),

                        new Vector3(x, 0f, z + 1f),
                        new Vector3(x + 1f, 0f, z + 1f),
                    };

                    renderer.AddPoints(points);
                }
            }

            stopwatch.Stop();

            Console.WriteLine($"Grid build time: {stopwatch.ElapsedMilliseconds}");

            return gameObject;
        }


        private void RegisterCameraPrefab()
        {
            var camera = new GameObject(this);

            camera.Transform.SetPosition(Vector3.UnitY * 3f);

            camera.AddTag("Main Camera");
            camera.AddComponent<Camera>();
            camera.AddComponent<CameraMovement>();

            Core.Resource.Save(new Prefab("cameraPrefab", camera));
        }

        public void OnFinish()
        {
            UnregisterAll();
        }
    }
}
