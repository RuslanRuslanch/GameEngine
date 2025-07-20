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
        private readonly HashSet<GameObject> _registerTargets = new HashSet<GameObject>();

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

        public void HandleRegisterRequests()
        {
            foreach (var target in _registerTargets)
            {
                Register(target);
            }

            _registerTargets.Clear();
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
            if (MainCamera == null)
            {
                return;
            }

            var renderTime = 0f;
            var cullingTime = 0f;

            var stopwatch = Stopwatch.StartNew();

            foreach (var gameObject in _gameObjects)
            {
                stopwatch.Restart();

                if (gameObject.CanRender(MainCamera.Frustum) == false)
                {
                    continue;
                }

                stopwatch.Stop();
                cullingTime += (float)stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Restart();

                gameObject.OnRender();

                stopwatch.Stop();
                renderTime += (float)stopwatch.Elapsed.TotalMilliseconds;
            }

            stopwatch.Stop();

            Console.WriteLine($"Render time: {renderTime} | Culling time: {cullingTime}");
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
            RegisterTestPrefab();

            var camera = Core.Resource.Get<Prefab>("CameraPrefab").GameObject;
            var test = Core.Resource.Get<Prefab>("TestPrefab").GameObject;

            var particle = SpawnParticle();
            var grid = SpawnGrid();

            var character = new GameObject(this);

            character.Transform.SetScale(new Vector3(1f, 2f, 0f));;

            character.AddComponent<SpriteRenderer>();
            character.AddComponent<CharacterMovement>();
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

            var renderer =  gameObject.AddComponent<ParticleRenderer>();

            renderer.Play();

            return gameObject;
        }

        private GameObject SpawnGrid()
        {
            var gameObject = new GameObject(this);

            var renderer = gameObject.AddComponent<LineRenderer>();
            var source = gameObject.AddComponent<SoundSource>();

            renderer.SetColor(Color4.GhostWhite);

            var stopwatch = Stopwatch.StartNew();

            var allPoints = new List<Vector3>(); 

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

                    allPoints.AddRange(points);
                }
            }

            renderer.SetPoints(allPoints.ToArray());

            stopwatch.Stop();

            Console.WriteLine($"Grid build time: {stopwatch.ElapsedMilliseconds}");

            return gameObject;
        }


        private void RegisterCameraPrefab()
        {
            var camera = new GameObject(this);

            camera.Transform.Move(Vector3.UnitY * 5f);
            camera.Transform.Move(Vector3.UnitZ * 15f);

            camera.AddTag("Main Camera");
            camera.AddComponent<Camera>();
            //camera.AddComponent<CameraMovement>();

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
