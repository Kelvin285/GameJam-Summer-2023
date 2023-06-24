using OpenTK.Mathematics;

namespace SandMan.game;

public class Camera
{
    public Vector2 position = new Vector2(0, 0);

    public Matrix4 projection = Matrix4.CreateOrthographic(1920, 1080, 0, 1);

    public void Update(Vector2i size)
    {
        projection = Matrix4.CreateOrthographic(size.X, size.Y, 0, 1);
    }
}