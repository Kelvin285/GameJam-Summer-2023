using DotnetNoise;
using OpenTK.Mathematics;
using SandMan.blocks;
using SandMan.rendering;

namespace SandMan.game;

public class LevelGeneration
{
    public Chunk[] chunks;
    private FastNoise noise;

    public LevelGeneration()
    {
        chunks = new Chunk[64*8];
        noise = new FastNoise();
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                chunks[x + y * 64] = new Chunk(x, y);
            }
        }
        for (int x = 0; x < 64*128; x++)
        {
            for (int y = 0; y < 64*128; y++)
            {
                float noise = this.noise.GetPerlin(x, y);
                noise += y * 0.01f;
                if (noise < 0f)
                {
                    SetBlock(x, y, BlockRegistry.sand);
                }
            }
        }
        
    }
    
    public Chunk GetChunk(int x, int y)
    {
        x &= 63;
        y &= 7;
        return chunks[x + y * 64];
    }

    public void SetBlock(int x, int y, Block block)
    {
        x &= 8191;
        y &= 8191;
        int chunkX = x / 128;
        int chunkY = y / 128;
        chunks[chunkX + chunkY * 64].SetBlock(x  & 127, y & 127, block);
    }

    public Block GetBlock(int x, int y)
    {
        x &= 8191;
        y &= 8191;
        int chunkX = x / 128;
        int chunkY = y / 128;
        return chunks[chunkX + chunkY * 64].GetBlock(x & 127, y & 127);
        
    }

    public void Render()
    {
        Camera camera = Game.INSTANCE.camera;
        int cameraX = (int)MathF.Floor(camera.position.X/128);
        int cameraY = (int)MathF.Floor(camera.position.Y/128);
        for (int x = cameraX - 3; x < cameraX + 3; x++)
        {
            for (int y = cameraX - 3; y < cameraY + 3; y++)
            {
                GetChunk(x, y).Render();
            }
        }
    }

    public void Dispose()
    {
        foreach (Chunk chunk in chunks)
        {
            chunk.chunkTexture.Dispose();
        }
    }
    
}