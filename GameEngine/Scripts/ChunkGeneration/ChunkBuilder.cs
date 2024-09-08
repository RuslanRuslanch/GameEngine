using GameEngine.Scripts.ChunkGeneration.Mesh;
using GameEngine.Scripts.ChunkGeneration.Terrain;

namespace GameEngine.Scripts.ChunkGeneration
{
    public sealed class ChunkBuilder
    {
        public static ChunkBuilder Instance { get; private set; }

        public readonly ChunkPolygonsBuilder PolygonsBuilder;
        public readonly TerrainBuilder TerrainBuilder;

        public ChunkBuilder(TerrainBuilder terrainBuilder)
        {
            if (Instance == null)
            {
                Instance = this;
            }

            PolygonsBuilder = new ChunkPolygonsBuilder();
            TerrainBuilder = terrainBuilder;
        }
    }
}
