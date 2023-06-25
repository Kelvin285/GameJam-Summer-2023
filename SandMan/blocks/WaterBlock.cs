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
        else
        {
            int place = x;

            void Search(int dir, int range = 5)
            {
                for (int i = 1; i < range; i++)
                {
                    if (world.GetBlock(x + i * dir, y) == BlockRegistry.air)
                    {
                        place = x + i * dir;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            int dir = world.random.Next(2) * 2 - 1;
            Search(dir);
            if (place == x)
            {
                Search(-dir);
            }

            if (place != x)
            {
                world.SetBlock(x, y, BlockRegistry.air);
                world.SetBlock(place, y, this);
                return true;
            }
            

            
        }
        return false;
    }
}