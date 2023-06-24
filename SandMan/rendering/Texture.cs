using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SandMan.rendering;

public class Texture
{
    public TextureHandle texture;

    public Vector4[] pixels;
    public int width, height;
    
    
    public Texture(int width, int height, Vector4 color)
    {
        this.width = width;
        this.height = height;
        pixels = new Vector4[width * height];
        Array.Fill(pixels, color);
        
        GL.BindTexture(TextureTarget.Texture2d, texture);
        GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, pixels.ToArray());
        GL.GenerateMipmap(TextureTarget.Texture2d);
    }

    public Texture(string path)
    {
        texture = GL.GenTexture();
        Image<Rgba32> image = Image.Load<Rgba32>(path);
        image.Mutate(x => x.Flip(FlipMode.Vertical));

        width = image.Width;
        height = image.Height;
        
        var pixels = new List<Vector4>(image.Width * image.Height);

        
        for (int y = 0; y < image.Height; y++)
        {
            PixelAccessorAction<Rgba32> action = (accessor) =>
            {
                var row = accessor.GetRowSpan(y);
                for (int x = 0; x < image.Width; x++)
                {
                    pixels.Add(new Vector4(row[x].R, row[x].G, row[x].B, row[x].A) / 255.0f);
                }
            };
            image.ProcessPixelRows(action);
        }

        this.pixels = pixels.ToArray();
        
        GL.BindTexture(TextureTarget.Texture2d, texture);
        GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba32f, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.Float, pixels.ToArray());
        GL.GenerateMipmap(TextureTarget.Texture2d);

        image.Dispose();
    }

    public void SetPixel(int x, int y, Vector4 pixel)
    {
        pixels[x + y * width] = pixel;
    }
    
    public Vector4 GetPixel(int x, int y)
    {
        return pixels[x + y * width];
    }

    public void Update()
    {
        GL.BindTexture(TextureTarget.Texture2d, texture);
        GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, pixels.ToArray());
        GL.GenerateMipmap(TextureTarget.Texture2d);
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