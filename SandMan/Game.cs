using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace SandMan;

public class Game : GameWindow
{
    public VertexArrayHandle vao;
    public Shader render_shader;

    public TextureHandle texture;
    
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

        vao = GL.CreateVertexArray();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.ClearColor(0, 0, 0, 0);

        render_shader.Use();
        
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
        GL.DeleteTexture(texture);
    }
}