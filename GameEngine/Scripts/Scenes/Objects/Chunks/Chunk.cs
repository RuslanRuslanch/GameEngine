using System.Net.Http.Headers;

namespace GameEngine.Scripts.Scenes.Objects.Chunks
{
    public sealed class Chunk : SceneObject
    {
        public const int Width = 16;
        public const int Height = 256; // Должно быть кратно двум, это нужно для работы сабчанков

        public const int BlockCount = Width * Height * Width;
    }
}
