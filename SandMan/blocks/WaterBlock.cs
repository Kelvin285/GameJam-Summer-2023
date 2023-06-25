using OpenTK.Mathematics;
using SandMan.game.world;

namespace SandMan.blocks;

public class WaterBlock : Block
{
    public WaterBlock(Vector4 color, bool solid = true, bool can_break = false) : base(color, solid, can_break)
    {
    }

    public WaterBlock(string texture, bool solid = true, bool can_break = false) : base(texture, solid, can_break)
    {
    }

    public override bool Update(int x, int y, BlockWorld world)
    {
        if (world.GetBlock(x, y - 1) == BlockRegistry.air)
        {
            world.SetBlock(x, y, BlockRegistry.air);
            world.SetBlock(x, y - 1, this);
            return true;
        }

        int placeX = x;
        int placeY = y;

        void Search(int dir, int range = 5)
        {
            for (int i = range; i >= 1; i--)
            {
                int newX = x + i * dir;
                if (world.GetBlock(newX, y) == BlockRegistry.air)
                {
                    placeX = newX;
                    placeY = y;
                    break;
                }
            }
        }

        int dir = world.random.Next(2) * 2 - 1;
        Search(dir);
        if (placeX == x)
        {
            Search(-dir);
        }

        if (placeX != x)
        {
            world.SetBlock(x, y, BlockRegistry.air);
            world.SetBlock(placeX, placeY, this);
        }
        return false;
    }
}