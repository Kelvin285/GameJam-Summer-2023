using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SandMan.rendering;

public unsafe class Shader
{
    public ProgramHandle program;
    
    public Shader(string vert_file, string frag_file)
    {
        string vert_code = File.ReadAllText(vert_file);
        string frag_code = File.ReadAllText(frag_file);

        program = GL.CreateProgram();

        ShaderHandle vert = GL.CreateShader(ShaderType.VertexShader);
        
        GL.ShaderSource(vert, vert_code);
        GL.CompileShader(vert);

        int success = 0;
        GL.GetShaderiv(vert, ShaderParameterName.CompileStatus, &success);

        if (success == 0)
        {
            string str = "";
            GL.GetShaderInfoLog(vert, out str);
            
            Console.WriteLine("Error ["+vert_file+"]: " + str);
        }

        ShaderHandle frag = GL.CreateShader(ShaderType.FragmentShader);
        
        GL.ShaderSource(frag, frag_code);
        GL.CompileShader(frag);

        GL.GetShaderiv(frag, ShaderParameterName.CompileStatus, &success);

        if (success == 0)
        {
            string str = "";
            GL.GetShaderInfoLog(frag, out str);
            
            Console.WriteLine("Error ["+frag_file+"]: " + str);
        }

        GL.UseProgram(program);
        GL.AttachShader(program, vert);
        GL.AttachShader(program, frag);
        GL.LinkProgram(program);
        
        GL.DeleteShader(vert);
        GL.DeleteShader(frag);
    }

    public void SetUniform(string name, bool value)
    {
        GL.Uniform1i(GL.GetUniformLocation(program, name), value ? 1 : 0);
    }
    
    public void SetUniform(string name, int value)
    {
        GL.Uniform1i(GL.GetUniformLocation(program, name), value);
    }
    
    public void SetUniform(string name, float value)
    {
        GL.Uniform1f(GL.GetUniformLocation(program, name), value);
    }
    
    public void SetUniform(string name, Vector2 value)
    {
        GL.Uniform2f(GL.GetUniformLocation(program, name), value);
    }
    
    public void SetUniform(string name, Vector3 value)
    {
        GL.Uniform3f(GL.GetUniformLocation(program, name), value);
    }
    
    public void SetUniform(string name, Vector4 value)
    {
        GL.Uniform4f(GL.GetUniformLocation(program, name), value);
    }
    
    public void SetUniform(string name, Matrix4 value)
    {
        GL.UniformMatrix4f(GL.GetUniformLocation(program, name), false, value);
    }
    
    public void Use()
    {
        GL.UseProgram(program);
    }

    public void Dispose()
    {
        GL.DeleteProgram(program);
    }
}