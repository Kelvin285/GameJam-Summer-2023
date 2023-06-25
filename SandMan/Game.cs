using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SandMan.game;
using SandMan.game.world;
using SandMan.rendering;

namespace SandMan;

public class Game : GameWindow
{
    public float Delta => (float)RenderTime;
    
    
    public VertexArrayHandle vao;
    public Shader render_shader;

    public Texture texture;

    public Camera camera = new Camera();

    public static Game INSTANCE;

    private BlockWorld _blockWorld;
    
    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        INSTANCE = this;
        Init();
    }

    public override void Run()
    {
        base.Run();
    }

    public void Init()
    {
        TextureRegistry.RegisterTextures();
        _blockWorld = new BlockWorld();
        
        render_shader = new Shader("assets/shaders/render_vert.glsl", "assets/shaders/render_frag.glsl");

        texture = new Texture("assets/textures/file.png");
        
        vao = GL.CreateVertexArray();
    }

    private float rotation = 0;
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        camera.Update(Size);
        
        base.OnRenderFrame(args);
        
        GL.Viewport(0, 0, Size.X, Size.Y);
        
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.ClearColor(0.75f, 0.9f, 1, 0);

        render_shader.Use();
        
        render_shader.SetUniform("camera", camera.position);
        render_shader.SetUniform("projection", camera.projection);
        
        //Render Functions Go Here
        render_shader.SetUniform("tex", 0);
        _blockWorld.Render();

        SwapBuffers();

    }
    
    public static void DrawTexture(Texture texture, Vector2 position, Vector2 size, float rotation = 0, bool centered = false)
    {
        Game game = Game.INSTANCE;
        texture.Bind();
        game.render_shader.SetUniform("position", position);
        game.render_shader.SetUniform("size", size);
        game.render_shader.SetUniform("model", Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation)));
        game.render_shader.SetUniform("centered", centered);
        GL.BindVertexArray(game.vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        _blockWorld.Update();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        
        
        render_shader.Dispose();
        _blockWorld.Dispose();
        
        GL.DeleteVertexArray(vao);
        texture.Dispose();
        TextureRegistry.Dispose();
    }

}