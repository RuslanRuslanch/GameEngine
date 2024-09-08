using GameEngine.Scripts.Meshes;
using GameEngine.Scripts.Scenes.Objects.Blocks;
using GameEngine.Scripts.Scenes.Objects.Chunks;
using OpenTK.Mathematics;

namespace GameEngine.Scripts.ChunkGeneration.Mesh
{
    public sealed class ChunkPolygonsBuilder
    {
        private List<Polygon> _polygons = new List<Polygon>();
        private BlockType[] _blocks = Array.Empty<BlockType>();

        private Vector3i _chunkPosition;

        public Polygon[] Get(BlockType[] blocks, Vector3i chunkPosition)
        {
            _polygons = new List<Polygon>();
            _blocks = blocks;
            _chunkPosition = chunkPosition;

            Console.WriteLine($"{blocks.Length} start with {blocks[0]}");

            for (int x = 0; x < Chunk.Width; x++)
            {
                for (int y = 0; y < Chunk.Height; y++)
                {
                    for (int z = 0; z < Chunk.Width; z++)
                    {
                        Polygon[] polygons = GeneratePolygons(x, y, z);

                        _polygons.AddRange(polygons);
                    }
                }
            }

            Polygon[] tempPolygons = _polygons.ToArray();

            _polygons = null;
            _blocks = null;

            return tempPolygons;
        }

        private BlockType Get(Vector3i blockPosition)
        {
            if (blockPosition.X >= 0 && blockPosition.X < Chunk.Width &&
                blockPosition.Y >= 0 && blockPosition.Y < Chunk.Height &&
                blockPosition.Z >= 0 && blockPosition.Z < Chunk.Width)
            {
                return _blocks[GetIndex(blockPosition)];
            }

            return BlockType.Air;
        }

        private Polygon[] GeneratePolygons(int x, int y, int z)
        {
            if (_blocks[GetIndex(x, y, z)] == BlockType.Air)
            {
                return Array.Empty<Polygon>();
            }

            List<Polygon> polygons = new List<Polygon>();

            Vector3i blockPosition = new Vector3i(x, y, z);

            if (Get(blockPosition + Vector3i.UnitX) == BlockType.Air)
            {
                polygons.AddRange(GenerateLeftFace(blockPosition + _chunkPosition));
            }
            if (Get(blockPosition - Vector3i.UnitX) == BlockType.Air)
            {
                polygons.AddRange(GenerateRightFace(blockPosition + _chunkPosition));
            }
            if (Get(blockPosition + Vector3i.UnitY) == BlockType.Air)
            {
                polygons.AddRange(GenerateTopFace(blockPosition + _chunkPosition));
            }
            if (blockPosition.Y > 0 && Get(blockPosition - Vector3i.UnitY) == BlockType.Air)
            {
                polygons.AddRange(GenerateBottomFace(blockPosition + _chunkPosition));
            }
            if (Get(blockPosition + Vector3i.UnitZ) == BlockType.Air)
            {
                polygons.AddRange(GenerateFrontFace(blockPosition + _chunkPosition));
            }
            if (Get(blockPosition - Vector3i.UnitZ) == BlockType.Air)
            {
                polygons.AddRange(GenerateBackFace(blockPosition + _chunkPosition));
            }

            return polygons.ToArray();
        }

        private Polygon[] GenerateFrontFace(Vector3i blockPosition)
        {
            Vector3[] vertices1 =
            {
                new Vector3(0f, 0f, 1f) + blockPosition,
                new Vector3(1f, 0f, 1f) + blockPosition,
                new Vector3(0f, 1f, 1f) + blockPosition,
            };

            Vector3[] vertices2 =
            {
                new Vector3(1f, 0f, 1f) + blockPosition,
                new Vector3(1f, 1f, 1f) + blockPosition,
                new Vector3(0f, 1f, 1f) + blockPosition,
            };

            return GenerateFace(vertices1, vertices2);
        }

        private Polygon[] GenerateBackFace(Vector3i blockPosition)
        {
            Vector3[] vertices1 =
            {
                new Vector3(0f, 0f, 0f) + blockPosition,
                new Vector3(0f, 1f, 0f) + blockPosition,
                new Vector3(1f, 0f, 0f) + blockPosition,
            };

            Vector3[] vertices2 =
            {
                new Vector3(1f, 0f, 0f) + blockPosition,
                new Vector3(0f, 1f, 0f) + blockPosition,
                new Vector3(1f, 1f, 0f) + blockPosition,
            };

            return GenerateFace(vertices1, vertices2);
        }

        private Polygon[] GenerateTopFace(Vector3i blockPosition)
        {
            Vector3[] vertices1 =
            {
                new Vector3(0f, 1f, 0f) + blockPosition,
                new Vector3(0f, 1f, 1f) + blockPosition,
                new Vector3(1f, 1f, 0f) + blockPosition,
            };

            Vector3[] vertices2 =
            {
                new Vector3(0f, 1f, 1f) + blockPosition,
                new Vector3(1f, 1f, 1f) + blockPosition,
                new Vector3(1f, 1f, 0f) + blockPosition,
            };

            return GenerateFace(vertices1, vertices2);
        }

        private Polygon[] GenerateBottomFace(Vector3i blockPosition)
        {
            Vector3[] vertices1 =
            {
                new Vector3(0f, 0f, 0f) + blockPosition,
                new Vector3(1f, 0f, 0f) + blockPosition,
                new Vector3(0f, 0f, 1f) + blockPosition,
            };

            Vector3[] vertices2 =
            {
                new Vector3(0f, 0f, 1f) + blockPosition,
                new Vector3(1f, 0f, 0f) + blockPosition,
                new Vector3(1f, 0f, 1f) + blockPosition,
            };

            return GenerateFace(vertices1, vertices2);
        }

        private Polygon[] GenerateRightFace(Vector3i blockPosition)
        {
            Vector3[] vertices1 =
            {
                new Vector3(0f, 0f, 1f) + blockPosition,
                new Vector3(0f, 1f, 1f) + blockPosition,
                new Vector3(0f, 0f, 0f) + blockPosition,
            };

            Vector3[] vertices2 =
            {
                new Vector3(0f, 1f, 1f) + blockPosition,
                new Vector3(0f, 1f, 0f) + blockPosition,
                new Vector3(0f, 0f, 0f) + blockPosition,
            };

            return GenerateFace(vertices1, vertices2);
        }

        private Polygon[] GenerateLeftFace(Vector3i blockPosition)
        {
            Vector3[] vertices1 =
            {
                new Vector3(1f, 0f, 1f) + blockPosition,
                new Vector3(1f, 0f, 0f) + blockPosition,
                new Vector3(1f, 1f, 1f) + blockPosition,
            };

            Vector3[] vertices2 =
            {
                new Vector3(1f, 1f, 1f) + blockPosition,
                new Vector3(1f, 0f, 0f) + blockPosition,
                new Vector3(1f, 1f, 0f) + blockPosition,
            };

            return GenerateFace(vertices1, vertices2);
        }

        private Polygon[] GenerateFace(Vector3[] vertices1, Vector3[] vertices2)
        {
            var firstPolygon = new Polygon(vertices1, null, Vector3.Zero);
            var secondPolygon = new Polygon(vertices2, null, Vector3.Zero);

            return new Polygon[] { firstPolygon, secondPolygon };
        }

        private int GetIndex(Vector3i blockPosition)
        {
            return blockPosition.X * Chunk.Height * Chunk.Width + blockPosition.Y * Chunk.Width + blockPosition.Z;
        }

        private int GetIndex(int x, int y, int z)
        {
            return x * Chunk.Height * Chunk.Width + y * Chunk.Width + z;
        }
    }
}
