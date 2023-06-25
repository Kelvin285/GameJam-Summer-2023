using DotnetNoise;
using SandMan.blocks;
using SandMan.game.entities;

namespace SandMan.game.world;

public class World
{
    public Chunk[] chunks = new Chunk[64 * 64];
    private FastNoise noise;

    public List<Entity> entities = new List<Entity>();

    public World()
    {
        noise = new FastNoise();
        
        entities.Add(new Player(this));
        
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                chunks[x + y * 64] = new Chunk(x, y);
            }
        }
        for (int x = 0; x < 128 * 64; x++)
        {
            float progress = x / (128 * 64.0f);

            string str = "";
            for (int i = 0; i < 100; i++)
            {
                if (i < progress * 100)
                {
                    str += "[]";
                }
                else
                {
                    str += "--";
                }
            }
            Console.WriteLine(str);
            
            float noise = this.noise.GetSimplexFractal(x * 0.2f, 0);

            float height = noise * 128 + 512;
            
            for (int y = 0; y < 128 * 64; y++)
            {
                if (y < height)
                {
                    SetBlock(x, y, BlockRegistry.sand);
                }
            }
        }
        
        // lake generation
        Random random = new Random();
        
        for (int i = 0; i < 1000; i++)
        {
            int x = random.Next(8192);
            int y = random.Next(384) + 384;

            int start_x = -1;
            int end_x = -1;
            int start_y = -1;
            for (int w = x; w > x - 128; w--)
            {
                if (GetBlock(w, y).solid)
                {
                    start_x = w;
                }
            }
            
            for (int w = x; w < x + 128; w++)
            {
                if (GetBlock(w, y).solid)
                {
                    end_x = w;
                }
            }

            for (int h = y; h > y - 64; h--)
            {
                if (GetBlock(x, h).solid)
                {
                    start_y = h;
                }
            }

            if (start_x < 0 || end_x < 0)
            {
                continue;
            }
            
            int[] depth = new int[end_x - start_x];
            for (int w = start_x; w < end_x; w++)
            {
                int index = w - start_x;
                
                for (int h = y; h > y - 64; h--)
                {
                    if (GetBlock(w, h).solid)
                    {
                        depth[index] = h;
                        break;
                    }
                }
            }

            if (start_y >= 0)
            {
                
                
                for (int w = start_x; w < end_x; w++)
                {
                    for (int h = start_y; h < y; h++)
                    {
                        if (GetBlock(w, h) == BlockRegistry.air)
                        {
                            SetBlock(w, h, BlockRegistry.water);
                        }
                    }
                }
            }
        }
    }
    
    public Chunk GetChunk(int x, int y)
    {
        x &= 63;
        y &= 63;
        return chunks[x + y * 64];
    }

    public void SetBlock(int x, int y, Block block)
    {
        x &= 8191;
        y &= 8191;
        int chunkX = x / 128;
        int chunkY = y / 128;
        chunks[chunkX + chunkY * 64].SetBlock(x, y, block);
    }

    public Block GetBlock(int x, int y)
    {
        x &= 8191;
        y &= 8191;
        int chunkX = x / 128;
        int chunkY = y / 128;
        return chunks[chunkX + chunkY * 64].GetBlock(x, y);
        
    }

    public void Update()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Update();
        }
    }
    
    public void Render()
    {
        Camera camera = Game.INSTANCE.camera;
        int cameraX = (int)MathF.Floor(camera.position.X/128);
        int cameraY = (int)MathF.Floor(camera.position.Y/128);

        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].Render();
        }

        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Render();
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