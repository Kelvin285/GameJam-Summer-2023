using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SandMan.rendering;

namespace SandMan;

public class Game : GameWindow
{
    public VertexArrayHandle vao;
    public Shader render_shader;

    public Texture texture;
    
    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        Init();
    }

    public override void Run()
    {
        base.Run();
    }

    public void Init()
    {
        render_shader = new Shader("assets/shaders/render_vert.glsl", "assets/shaders/render_frag.glsl");

        texture = new Texture("assets/textures/file.png");
        
        vao = GL.CreateVertexArray();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        
        GL.Viewport(0, 0, Size.X, Size.Y);
        
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.ClearColor(0, 0, 0, 0);

        render_shader.Use();

        texture.Bind();
        render_shader.SetUniform("tex", 0);
        
        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        
        SwapBuffers();

    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        
        
        render_shader.Dispose();
        
        GL.DeleteVertexArray(vao);
        texture.Dispose();
    }
}