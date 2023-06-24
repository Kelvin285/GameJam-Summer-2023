using OpenTK.Mathematics;

namespace SandMan.blocks;

public class Block
{
    
    public Vector4 color;
    public int id;

    private static int currentId;

    public Block(Vector4 color)
    {
        this.color = color;
        id = currentId++;
        BlockRegistry.blocks.Add(this);
    }

}

