using OpenTK.Graphics;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace SandMan;

public class Texture
{
    public TextureHandle texture;
    public Texture(int width, int height, Vector4 color)
    {
        
    }

    public Texture(string path)
    {
        texture = GL.GenTexture();
        Image<Rgba32> image = Image.Load<Rgba32>(path);
        image.Mutate(x => x.Flip(FlipMode.Vertical));
        var pixels = new byte[4 * image.Width * image.Height];
        image.CopyPixelDataTo(pixels);
        
        GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
    }

    public void Dispose()
    {
        GL.DeleteTexture(texture);
    }
}