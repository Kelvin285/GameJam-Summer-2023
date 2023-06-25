using OpenTK.Mathematics;
using SandMan.game.world;
using SandMan.rendering;

namespace SandMan.game.entities;

public class Enemy : Entity
{
    public Vector2 size;
    public Enemy(BlockWorld world) : base(world, new Vector2(4096, 0))
    {
        int tallest = 0;
        for (int y = 0; y < 8192; y++)
        {
            if (world.GetBlock(4096, y).solid && !world.GetBlock(4096, y + 1).solid)
            {
                if (y > tallest)
                {
                    tallest = y;
                }
            }
        }

        position.X = 4096;
        position.Y = tallest;
        size = new Vector2(TextureRegistry.ENEMY.width, TextureRegistry.ENEMY.height / 20) * 0.5f;
    }

    public override void Update()
    {
        base.Update();
        
        int x = (int)position.X;
        int y = (int)position.Y;
        int width = (int)size.X;
        int height = (int)size.Y;

        bool on_ground = world.GetBlock(x + width / 2, y).solid;

        if (!on_ground)
        {
            if (motion.Y > -9.81f)
            {
                motion.Y -= Game.INSTANCE.Delta * 4.0f;
            }
        }
        else
        {
            if (motion.Y < 0)
            {
                motion.Y = 0;
            }

            if (world.GetBlock(x + width / 2, y + 1).solid)
            {
                position.Y += 1;
            }
        }

        position += motion * Game.INSTANCE.Delta * 60;
    }

    public override void Render()
    {
        base.Render();
        float h = 1.0f / 20.0f;
        Game.DrawTexture(TextureRegistry.ENEMY, position - size * 0.5f, size, new Vector4(0, h * 0, 1, h), 0, true);
        
        Game.DrawTexture(TextureRegistry.SHARK, position, new Vector2(TextureRegistry.SHARK.width, TextureRegistry.SHARK.height) / 2, 0, true);
    }
}