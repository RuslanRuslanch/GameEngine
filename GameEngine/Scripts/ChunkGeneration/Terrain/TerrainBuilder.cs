using GameEngine.Scripts.Scenes.Objects.Blocks;
using OpenTK.Mathematics;

namespace GameEngine.Scripts.ChunkGeneration.Terrain
{
    public abstract class TerrainBuilder
    {
        public abstract BlockType[] Get(Vector2i localPosition);
    }
}
