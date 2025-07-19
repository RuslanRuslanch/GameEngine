using System.Diagnostics;
using GameEngine.Components;
using GameEngine.GameObjects;
using GameEngine.Resources;
using OpenTK.Mathematics;

namespace GameEngine.Worlds
{
    public sealed class World
    {
        private readonly List<GameObject> _gameObjects = new List<GameObject>();
        private readonly List<GameObject> _registerTargets = new List<GameObject>();

        public readonly Core Core;

        public Camera MainCamera { get; private set; }

        public World(Core core)
        {
            Core = core;
        }

        public void SendRegisterRequest(GameObject gameObject)
        {
            if (_registerTargets.Contains(gameObject))
            {
                return;
            }

            _registerTargets.Add(gameObject);
        }

        private void Register(GameObject gameObject)
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

        private void Unregister(GameObject gameObject)
        {
            gameObject.OnFinish();

            _gameObjects.Remove(gameObject);
        }

        public void UnregisterAll()
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                Unregister(_gameObjects[i]);
            }
        }

        public T FindByType<T>() where T : Component
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                if (_gameObjects[i].TryGetComponent(out T result))
                {
                    return result;
                }
            }

            return null;
        }

        public GameObject FindByTag(string tag)
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                if (_gameObjects[i].HasTag(tag))
                {
                    return _gameObjects[i];
                }
            }

            return null;
        }

        public void HandleRegisterRequests()
        {
            for (int i = 0; i < _registerTargets.Count; i++)
            {
                Register(_registerTargets[i]);

                _registerTargets.RemoveAt(i);
            }
        }

        public void OnUpdate(float delta)
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                _gameObjects[i].OnUpdate(delta);
            }
        }

        public void OnRender()
        {
            if (MainCamera == null)
            {
                return;
            }

            var renderTime = 0f;
            var cullingTime = 0f;

            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < _gameObjects.Count; i++)
            {
                stopwatch.Restart();

                if (_gameObjects[i].CanRender(MainCamera.Frustum) == false)
                {
                    continue;
                }

                stopwatch.Stop();
                cullingTime += (float)stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Restart();

                _gameObjects[i].OnRender();

                stopwatch.Stop();
                renderTime += (float)stopwatch.Elapsed.TotalMilliseconds;
            }

            //stopwatch.Stop();

            //Console.WriteLine($"Render time: {renderTime} | Culling time: {cullingTime}");
        }

        public void OnTick()
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                _gameObjects[i].OnTick();
            }
        }

        public void OnStart()
        {
            RegisterCameraPrefab();
            RegisterTestPrefab();

            var camera = Core.Resource.Get<Prefab>("CameraPrefab").GameObject;
            var test = Core.Resource.Get<Prefab>("TestPrefab").GameObject;

            var particle = SpawnParticle();
            var grid = SpawnGrid();

            var character = new GameObject(this);

            character.Transform.SetScale(new Vector3(1f, 2f, 1f));
            character.Transform.Move(Vector3.UnitX * 10f);

            character.AddComponent<SpriteRenderer>();
            character.AddComponent<ObjectSpawner>();

            SendRegisterRequest(character);
            SendRegisterRequest(grid);
            SendRegisterRequest(particle);
            SendRegisterRequest(camera);
            SendRegisterRequest(test);
        }

        public void OnFinish()
        {
            UnregisterAll();
        }

        private GameObject SpawnParticle()
        {
            var gameObject = new GameObject(this);

            gameObject.AddComponent<ParticleRenderer>().Play();

            return gameObject;
        }

        private GameObject SpawnGrid()
        {
            var gameObject = new GameObject(this);

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

            camera.Transform.Move(Vector3.UnitY * 3f);
            camera.Transform.Move(Vector3.UnitZ * 5f);

            camera.AddTag("Main Camera");
            camera.AddComponent<Camera>();
            camera.AddComponent<CameraMovement>();

            Core.Resource.Save(new Prefab("CameraPrefab", camera));
        }

        private void RegisterTestPrefab()
        {
            var gameObject = new GameObject(this);

            //gameObject.AddComponent<LineRenderer>().SetPoints(new Vector3[] {Vector3.Zero, Vector3.UnitX});
            
            gameObject.AddComponent<TextRenderer>();

            Core.Resource.Save(new Prefab("TestPrefab", gameObject));
        }
    }
}
