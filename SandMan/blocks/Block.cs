using OpenTK.Mathematics;
using SandMan.game.world;
using SandMan.rendering;

namespace SandMan.blocks;

public class Block
{
    public int id;

    public bool solid = true;
    public bool can_break = true;

    private static int currentId;

    public Texture texture;

    public Block(Vector4 color, bool solid = true, bool can_break = false)
    {
        this.can_break = can_break;
        this.texture = new Texture(1, 1, color);
        id = currentId++;
        this.solid = solid;
        BlockRegistry.blocks.Add(this);
    }
    
    public Block(string texture, bool solid = true, bool can_break = false)
    {
        this.can_break = can_break;
        this.texture = new Texture("assets/textures/blocks/"+texture);
        id = currentId++;
        this.solid = solid;
        BlockRegistry.blocks.Add(this);
    }

    public Vector4 GetColor(int x, int y)
    {
        if (x < 0) x = 0;
        if (y < 0) y = 0;
        return texture.GetPixel(x % texture.width, y % texture.height);
    }

    public virtual bool Update(int x, int y, BlockWorld world)
    {
        return false;
    }

}

