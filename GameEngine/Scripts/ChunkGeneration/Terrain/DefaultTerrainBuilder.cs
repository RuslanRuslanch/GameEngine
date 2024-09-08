using GameEngine.Scripts.Scenes.Objects.Blocks;
using GameEngine.Scripts.Scenes.Objects.Chunks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GameEngine.Scripts.ChunkGeneration.Terrain
{
    public class DefaultTerrainBuilder : TerrainBuilder
    {
        private readonly FastNoiseLite _noise;

        private readonly NoiseSettings _noiseSettings;

        public DefaultTerrainBuilder(NoiseSettings noiseSettings)
        {
            _noiseSettings = noiseSettings;

            _noise = new FastNoiseLite();

            ApplyNoiseSettings(noiseSettings, ref _noise);
        }

        private void ApplyNoiseSettings(NoiseSettings noiseSettings, ref FastNoiseLite noise)
        {
            noise.SetRotationType3D(noiseSettings.RotationType);
            noise.SetFractalType(noiseSettings.FractalType);
            noise.SetNoiseType(noiseSettings.NoiseType);
            noise.SetFrequency(noiseSettings.Frequency);
        }

        public override BlockType[] Get(Vector2i localPosition)
        {
            BlockType[] blocks = new BlockType[Chunk.BlockCount];

            localPosition *= Chunk.Width;

            for (int x = 0; x < Chunk.Width; x++)
            {
                for (int z = 0; z < Chunk.Width; z++)
                {
                    float height = _noise.GetNoise(
                        (x + localPosition.X) * _noiseSettings.Amplitude,
                        (z + localPosition.Y) * _noiseSettings.Amplitude) * _noiseSettings.Depth + 16;

                    for (int y = 0; y < height; y++)
                    {
                        int index = x * Chunk.Height * Chunk.Width + y * Chunk.Width + z;

                        blocks[index] = BlockType.Grass;
                    }
                }
            }

            return blocks;
        }
    }
}
