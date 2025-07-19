using GameEngine.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GameEngine.Resources
{
    public sealed class Font : IResource
    {
        private readonly Dictionary<char, int> _vaos = new Dictionary<char, int>();

        private readonly string _alphabet;

        public readonly Texture Texture;
        public readonly Shader Shader;
        public readonly Vector2 LetterSize;

        public ResourceType Type => ResourceType.Font;
        public string ID { get; private set; }

        public Font(string id, string alphabet, Texture texture, Shader shader, Vector2 letterSize)
        {
            ID = id;
            LetterSize = letterSize;
            Shader = shader;
            Texture = texture;

            _alphabet = alphabet;
        }

        public void Generate(Resource resource)
        {
            for (int i = 0; i < _alphabet.Length; i++)
            {
                GenerateLetter(i, _alphabet[i], resource);
            }
        }

        private void GenerateLetter(int index, char letter, Resource resource)
        {
            var mesh = resource.Get<Mesh>("SpriteMesh");

            var start = Vector2.UnitX * LetterSize.X * index;
            var end = start + LetterSize;

            var uvs = new Vector2[]
            {
                start,
                start + Vector2.UnitX * LetterSize,
                start + Vector2.UnitY * LetterSize,
                end,
            };

            var ebo = new EBO(mesh.Indecies, BufferUsageHint.StaticDraw).ID;
            var vertexObject = new VBO(mesh.Vertices, BufferUsageHint.StaticDraw).ID;
            var uvObject = new VBO(uvs, BufferUsageHint.StaticDraw).ID;
            var normalObject = new VBO(mesh.Normals, BufferUsageHint.StaticDraw).ID;
            var vao = new DefaultVAO(ebo, vertexObject, uvObject, normalObject, Shader).ID;

            _vaos.Add(letter, vao);
        }

        public void Delete()
        {
            Texture.Delete();
        }

        public int Get(char letter)
        {
            return _vaos[letter];
        }
    }
}
