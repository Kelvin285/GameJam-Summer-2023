using OpenTK.Mathematics;

namespace SandMan.game;

public class Camera
{
    public Vector2 position = new Vector2(0, 0);

    public Matrix4 projection = Matrix4.CreateOrthographic(1920, 1080, 0, 1);

    public void Update(Vector2i size)
    {
        projection = Matrix4.CreateOrthographic(size.X / 2.0f, size.Y / 2.0f, 0, 1);
    }
    
    public Vector2i GetMousePositionInWorld()
    {
        Vector2 mousePos = Game.INSTANCE.MousePosition;
        mousePos /= Game.INSTANCE.Size;

        mousePos -= Vector2.One * 0.5f;
        mousePos *= 2.0f;
        mousePos.Y *= -1.0f;

        Vector2 worldSpace = (projection.Inverted() * new Vector4(mousePos)).Xy + position;

        return new Vector2i((int)MathF.Floor(worldSpace.X), (int)MathF.Floor(worldSpace.Y));
    }
    
}