using GameEngine.Scripts.ChunkGeneration;
using GameEngine.Scripts.ChunkGeneration.Terrain;
using GameEngine.Scripts.Meshes;
using GameEngine.Scripts.Scenes.Objects.Blocks;
using GameEngine.Scripts.Scenes.Objects.Chunks;
using GameEngine.Scripts.Scenes.Objects.Components.Rendering;
using OpenTK.Mathematics;
using static System.Reflection.Metadata.BlobBuilder;

namespace GameEngine.Scripts.Scenes.Objects.Components.World
{
    public sealed class WorldGenerator : Component
    {
        private readonly Dictionary<Vector2i, Chunk> _spawnedChunks = new Dictionary<Vector2i, Chunk>();

        private readonly Queue<LoadData> _loadingChunks = new Queue<LoadData>();

        private readonly Scene _scene;
        private readonly Transform _playerTransform;
        private readonly ChunkBuilder _chunkBuilder;

        private Vector2i _lastPosition;

        private Vector2i PlayerChunkPosition => new Vector2i((int)_playerTransform.Position.X, (int)_playerTransform.Position.Z) / Chunk.Width;

        public WorldGenerator(Transform playerTransform, Scene scene)
        {
            _playerTransform = playerTransform;
            _scene = scene;

            NoiseSettings noiseSettings = new NoiseSettings()
            {
                FractalType = FastNoiseLite.FractalType.None,
                NoiseType = FastNoiseLite.NoiseType.OpenSimplex2,
                RotationType = FastNoiseLite.RotationType3D.None,

                Depth = 5f,
                Amplitude = 0.14f,
                Frequency = 0.4f,
            };

            TerrainBuilder terrainBuilder = new DefaultTerrainBuilder(noiseSettings);

            _chunkBuilder = new ChunkBuilder(terrainBuilder);
        }

        public override void Load()
        {
            LoadChunks(PlayerChunkPosition);
            TrySpawnChunks();
        }

        public override void Update()
        {
            TryLoad();
            TrySpawnChunks();
        }

        private bool TryLoad()
        {
            Vector2i currentPosition = PlayerChunkPosition;

            if (_lastPosition != currentPosition)
            {
                LoadChunks(currentPosition);

                _lastPosition = currentPosition;

                return true;
            }

            return false;
        }

        private void LoadChunks(Vector2i playerPosition)
        {
            for (int x = playerPosition.X - Camera.ViewRadius; x < playerPosition.X + Camera.ViewRadius; x++)
            {
                for (int y = playerPosition.Y - Camera.ViewRadius; y < playerPosition.Y + Camera.ViewRadius; y++)
                {
                    Vector2i chunkPosition = new Vector2i(x, y);

                    if (_spawnedChunks.ContainsKey(chunkPosition))
                    {
                        continue;
                    }

                    LoadChunk(chunkPosition);
                }
            }
        }

        private void LoadChunk(Vector2i localPosition)
        {   
            LoadData data = new LoadData();

            Vector3i worldPosition = new Vector3i(localPosition.X, 0, localPosition.Y) * Chunk.Width;

            BlockType[] blocks = _chunkBuilder.TerrainBuilder.Get(localPosition);
            Polygon[] polygons = _chunkBuilder.PolygonsBuilder.Get(blocks, worldPosition);

            data.Polygons = polygons;

            data.LocalPosition = localPosition;
            data.WorldPosition = worldPosition;

            data.State = LoadState.Loaded;

            _loadingChunks.Enqueue(data);
        }

        private Chunk SpawnChunk(ref LoadData data)
        {
            Chunk chunk = new Chunk();
            Mesh mesh = new Mesh();

            mesh.SetPolygons(data.Polygons);

            Renderer renderer = new MeshRenderer(mesh);
            Transform transform = new Transform(data.WorldPosition, Vector3.One, Vector3.Zero);

            chunk.AddComponent(transform);
            chunk.AddComponent(renderer);

            return chunk;
        }

        private void TrySpawnChunks()
        {
            if (_loadingChunks.Any(data => data.State != LoadState.Loaded))
            {
                return;
            }

            SpawnChunks();
        }

        private void SpawnChunks()
        {
            while (_loadingChunks.TryDequeue(out LoadData data))
            {
                Chunk chunk = SpawnChunk(ref data);

                _scene.AddObject(chunk);
                _spawnedChunks.Add(data.LocalPosition, chunk);
            }
        }

        private struct LoadData
        {
            public LoadState State;

            public Vector2i LocalPosition;
            public Vector3i WorldPosition;

            public Polygon[] Polygons;
        }

        private enum LoadState : byte
        {
            Start,
            Loaded,
        }
    }
}
