using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GameEngine.Resources
{
    public sealed class Shader : IResource
    {
        public ResourceType Type => ResourceType.Shader;

        public string ID { get; private set; }
        public int GLObject { get; private set; }

        public Shader(string id, string vertexPath, string fragmentPath)
        {
            ID = id;
            GLObject = GL.CreateProgram();

            var vertex = CreateShader(ShaderType.VertexShader, vertexPath);
            var fragment = CreateShader(ShaderType.FragmentShader, fragmentPath);

            GL.AttachShader(GLObject, vertex);
            GL.AttachShader(GLObject, fragment);

            GL.LinkProgram(GLObject);

            GL.GetProgram(GLObject, GetProgramParameterName.LinkStatus, out int error);

            if (error != (int)All.True)
            {
                throw new Exception("Программа не слинкована!");
            }

            GL.DeleteShader(vertex);
            GL.DeleteShader(fragment);
        }

        private int CreateShader(ShaderType type, string path)
        {
            var shaderCode = File.ReadAllText(path);
            var shader = GL.CreateShader(type);

            GL.ShaderSource(shader, shaderCode);

            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int error);

            if (error != (int)All.True)
            {
                throw new Exception("Шейдер не скомпилирован!");
            }

            return shader;
        }

        public void Load(string name, ref Matrix4 matrix)
        {
            GL.ProgramUniformMatrix4(GLObject, GL.GetUniformLocation(GLObject, name), false, ref matrix);
        }

        public void Load(string name, ref Color4 color)
        {
            GL.ProgramUniform4(GLObject, GL.GetUniformLocation(GLObject, name), color);
        }

        public void Load(string name, ref Vector3 vector)
        {
            GL.ProgramUniform3(GLObject, GL.GetUniformLocation(GLObject, name), ref vector);
        }

        public void Load(string name, ref Vector2 vector)
        {
            GL.ProgramUniform2(GLObject, GL.GetUniformLocation(GLObject, name), ref vector);
        }

        public void Load(string name, int number)
        {
            GL.ProgramUniform1(GLObject, GL.GetUniformLocation(GLObject, name), number);
        }

        public int GetLocation(string name)
        {
            return GL.GetAttribLocation(GLObject, name);
        }

        public void Bind()
        {
            GL.UseProgram(GLObject);
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }

        public void Delete()
        {
            Unbind();
            GL.DeleteProgram(GLObject);
        }

    }
}
