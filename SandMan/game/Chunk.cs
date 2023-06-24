using OpenTK.Mathematics;
using SandMan.blocks;
using SandMan.rendering;

namespace SandMan.game;

public class Chunk
{
    public Texture chunkTexture;
    public List<int> blocks = new List<int>();
    public int x;
    public int y;

    private bool updated = true;

    public Chunk(int x, int y)
    {
        chunkTexture = new Texture(128, 128, new Vector4(0f, 0f, 0f, 1f));
        for (int i = 0; i < 128 * 128; i++)
        {
            blocks.Add(0);
        }
        this.x = x;
        this.y = y;
    }

    public void SetBlock(int x, int y, Block block)
    {
        updated = true;
        blocks[x + y * 128] = block.id;
        chunkTexture.SetPixel(x, y, block.color);
    }
    
    public Block GetBlock(int x, int y)
    {
        return BlockRegistry.blocks[blocks[x + y * 128]];
    }

    public void Render()
    {
        if (updated)
        {
            updated = false;
            chunkTexture.Update(); 
        }
        Game.DrawTexture(chunkTexture, new Vector2(x, y)*128, new Vector2(128, 128));
    }
    
}