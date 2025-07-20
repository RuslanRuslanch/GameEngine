using GameEngine.Resources;

namespace GameEngine.FileSystems
{
    public class FileSystem
    {
        private readonly Resource _resource;

        public FileSystem(Resource resource)
        {
            _resource = resource;
        }

        public IResource[] LoadResources(string path)
        {
            if (Directory.Exists(path) == false)
            {
                throw new DirectoryNotFoundException($"Directory isn't exists | Path: {path}");
            }

            var resources = new List<IResource>();
            var filePaths = Directory.GetFiles(path, "*.yml");

            Console.WriteLine("Files " + filePaths.Length);

            foreach (var filePath in filePaths)
            {
                var resource = LoadResource(filePath);

                if (resource == null)
                {
                    continue;
                }

                resources.Add(resource);
            }

            return resources.ToArray();
        }

        public IResource LoadResource(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            if (FindParameter("type:", lines, out var type) == false)
            {
                throw new KeyNotFoundException("'type:' not found");
            }

            Console.WriteLine(filePath);

            if (type == "texture")
            {
                if (FindParameter("path:", lines, out var path))
                {
                    path = path.Replace('"', ' ').Trim();

                    return _resource.SaveAndLoad(Path.GetFileNameWithoutExtension(filePath), ResourceType.Texture, $@"{path}");
                }
            }
            else if (type == "sound")
            {
                if (FindParameter("path:", lines, out var path))
                {
                    path = path.Replace('"', ' ').Trim();

                    return _resource.SaveAndLoad(Path.GetFileNameWithoutExtension(filePath), ResourceType.Sound, $@"{path}");
                }
            }
            else if (type == "material")
            {
                if (FindParameter("shader", lines, out var shaderPath) && 
                    FindParameter("texture", lines, out var texturePath))
                {
                    shaderPath = shaderPath.Replace('"', ' ').Trim();
                    texturePath = texturePath.Replace('"', ' ').Trim();

                    var shaderID = Path.GetFileNameWithoutExtension(shaderPath);
                    var textureID = Path.GetFileNameWithoutExtension(texturePath);
                    
                    if (_resource.Has(shaderID) == false)
                    {
                        LoadResource($@"{shaderPath}");
                    }
                    if (_resource.Has(textureID) == false)
                    {
                        LoadResource($@"{texturePath}");
                    }
                    
                    return _resource.SaveAndLoad(Path.GetFileNameWithoutExtension(filePath), ResourceType.Material, shaderID, textureID);
                }
            }
            else if (type == "shader")
            {
                if (
                    FindParameter("vertex_shader_path:", lines, out var vertex) &&
                    FindParameter("fragment_shader_path:", lines, out var fragment))
                {
                    vertex = vertex.Replace('"', ' ').Trim();
                    fragment = fragment.Replace('"', ' ').Trim();

                    return _resource.SaveAndLoad(Path.GetFileNameWithoutExtension(filePath), ResourceType.Shader, $@"{vertex}", $@"{fragment}");
                }
            }

            return null;
        }

        private bool FindParameter(string target, string[] lines, out string result)
        {
            result = string.Empty;

            foreach (var line in lines)
            {
                if (line.StartsWith(target))
                {
                    result = line.Split()[1];

                    return true;
                }
            }

            return false;
        }
    }
}