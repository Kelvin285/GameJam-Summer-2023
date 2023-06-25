using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SandMan.blocks;
using SandMan.game.world;

namespace SandMan.game.entities;

public class Player : Entity
{
    public Player(BlockWorld blockWorld, Vector2 position) : base(blockWorld, position)
    {
        
    }
    
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

        Vector2i mousePos = Game.INSTANCE.camera.GetMousePositionInWorld();
        bool mouse_press = Game.INSTANCE.IsMouseButtonPressed(MouseButton.Button1);
        
        //BlockWorld.SetBlock(mousePos.X, mousePos.Y, BlockRegistry.air);
        if (mouse_press)
        {
            world.CreateChunkEntity(mousePos + new Vector2i(0, -5), 28, mousePos + new Vector2(0, 16), new Vector2(0, 15), true);
            world.CreateChunkEntity(mousePos + new Vector2i(-15, 0), 20, mousePos + new Vector2(-15, 16), new Vector2(-25,15), true);
            world.CreateChunkEntity(mousePos + new Vector2i(15, 0), 20, mousePos + new Vector2(15, 16), new Vector2(25, 15), true);
        }
    }

    public override void Render()
    {
        base.Render();
    }
}