using GameEngine.Scripts.Scenes.Objects;
using GameEngine.Scripts.Scenes.Objects.Components;
using GameEngine.Scripts.Scenes.Objects.Components.Cameries;
using GameEngine.Scripts.Scenes.Objects.Components.World;
using OpenTK.Mathematics;

namespace GameEngine.Scripts.Scenes
{
    public class Scene
    {
        private readonly List<SceneObject> _objects = new List<SceneObject>();

        public void Load()
        {
            SpawnCamera();

            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].Load();
            }
        }

        public void Unload()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].Unload();
            }
        }

        public void Update()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].Update();
            }
        }

        public void Render()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].Render();
            }
        }

        public void AddObject(SceneObject sceneObject)
        {
            if (sceneObject == null)
            {
                throw new NullReferenceException(nameof(sceneObject));
            }
            if (_objects.Contains(sceneObject))
            {
                return;
            }

            _objects.Add(sceneObject);
        }

        public void RemoveObject(SceneObject sceneObject)
        {
            if (sceneObject == null)
            {
                throw new NullReferenceException(nameof(sceneObject));
            }
            if (_objects.Contains(sceneObject) == false)
            {
                throw new KeyNotFoundException(nameof(sceneObject));
            }

            _objects.Remove(sceneObject);
        }

        private void SpawnCamera()
        {
            Transform transform = new Transform(Vector3.Zero, Vector3.One, Vector3.UnitX * -90f);
            CameraMovement movement = new CameraMovement(10f, 0.5f, transform);
            WorldGenerator generator = new WorldGenerator(transform, this);

            Camera camera = new Camera();

            camera.AddComponent(transform);
            camera.AddComponent(generator);
            camera.AddComponent(movement);

            _objects.Add(camera);
        }
    }
}
