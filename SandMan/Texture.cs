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

        var pixels = new List<byte>(4 * image.Width * image.Height);


        for (int y = 0; y < image.Height; y++)
        {
            PixelAccessorAction<Rgba32> action = (accessor) =>
            {
                var row = accessor.GetRowSpan(y);
                for (int x = 0; x < image.Width; x++)
                {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            };
            image.ProcessPixelRows(action);
        }
        
        GL.BindTexture(TextureTarget.Texture2d, texture);
        GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
        GL.GenerateMipmap(TextureTarget.Texture2d);

        image.Dispose();
    }

    public void Bind()
    {
        GL.BindTexture(TextureTarget.Texture2d, texture);
    }
    
    public void Dispose()
    {
        GL.DeleteTexture(texture);
    }
}