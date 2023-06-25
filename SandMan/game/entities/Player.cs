using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SandMan.game.entities;

public class Player : Entity
{
    public override void Update()
    {
        base.Update();
        Game.INSTANCE.camera.position = position;

        bool IsKeyDown(Keys key) => Game.INSTANCE.IsKeyDown(key);

        float delta = Game.INSTANCE.Delta;

        float speed = 16 * 16;
        
        if (IsKeyDown(Keys.A))
        {
            position.X -= delta * speed;
        }

        if (IsKeyDown(Keys.D))
        {
            position.X += delta * speed;
        }

        if (IsKeyDown(Keys.W))
        {
            position.Y += delta * speed;
        }

        if (IsKeyDown(Keys.S))
        {
            position.Y -= delta * speed;
        }
    }

    public override void Render()
    {
        base.Render();
    }
}