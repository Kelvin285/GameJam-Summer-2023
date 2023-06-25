using OpenTK.Mathematics;
using SandMan.blocks;
using SandMan.rendering;

namespace SandMan.game.world;

public class Chunk
{
    public Texture chunkTexture;
    public List<int> blocks = new List<int>();
    public int x;
    public int y;

    private bool updated = true;

    public Chunk(int x, int y)
    {
        chunkTexture = new Texture(128, 128, new Vector4(0f, 0f, 0f, 0f));
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
        blocks[(x & 127) + (y & 127) * 128] = block.id;
        chunkTexture.SetPixel(x & 127, y & 127, block.GetColor(x, y));
    }
    
    public Block GetBlock(int x, int y)
    {
        return BlockRegistry.blocks[blocks[(x & 127) + (y & 127) * 128]];
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