using OpenTK.Mathematics;
using SandMan.blocks;
using SandMan.rendering;

namespace SandMan.game;

public class Chunk
{
    public Texture chunkTexture;
    public int[] blocks;
    public int x;
    public int y;

    private bool updated = false;

    public Chunk(int x, int y)
    {
        chunkTexture = new Texture(128, 128, new Vector4(0f, 0f, 0f, 1f));
        blocks = new int[128*128];
        this.x = x;
        this.y = y;
    }

    public void SetBlock(int x, int y, Block block)
    {
        blocks[y * 128 + x] = block.id;
        chunkTexture.SetPixel(x, y, block.color);
        updated = true;
    }
    
    public Block GetBlock(int x, int y)
    {
        return BlockRegistry.blocks[y * 128 + x];
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