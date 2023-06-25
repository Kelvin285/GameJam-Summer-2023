using Box2DSharp.Dynamics;
using DotnetNoise;
using OpenTK.Mathematics;
using SandMan.blocks;
using SandMan.game.entities;
using SandMan.rendering;

namespace SandMan.game.world;

public class BlockWorld
{
    public Chunk[] chunks = new Chunk[64 * 64];
    private FastNoise noise;

    public World physicsWorld;


    public Player player;
    public List<Entity> entities = new List<Entity>();

    public bool generated = false;

    public Random random = new Random();

    public BlockWorld()
    {
        physicsWorld = new World();
        physicsWorld.Gravity = new(0, -9.81f * 16.0f);
        
        noise = new FastNoise();

        player = new Player(this, Vector2.Zero);

        player.position.X = 4096;
        player.position.Y = 4096;
        
        entities.Add(player);
        
        GenerateLevel();
        generated = true;
    }

    public void GenerateLevel()
    {
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                chunks[x + y * 64] = new Chunk(this, x, y);
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


            float height = this.noise.GetSimplexFractal(x * 0.1f, 0) * 256;
            for (int y = 0; y < 128 * 64; y++)
            {
                float noise = this.noise.GetSimplexFractal(x * 0.25f, y * 0.25f);

                noise += (y + height - 512) / 128.0f;

                if (y < 512)
                {
                    SetBlock(x, y, BlockRegistry.water);
                }
                if (noise < 0)
                {
                    SetBlock(x, y, BlockRegistry.sand);
                }
            }
        }
        
        // lake generation
        Random random = new Random();
        
        for (int x = 0; x < 8192; x++)
        {
            int y = random.Next(512 - 384, 512 + 384);

            int start_x = -1;
            int end_x = -1;
            int start_y = -1;
            for (int w = x; w > x - 512; w--)
            {
                if (GetBlock(w, y).solid)
                {
                    start_x = w;
                }
            }
            
            for (int w = x; w < x + 512; w++)
            {
                if (GetBlock(w, y).solid)
                {
                    end_x = w;
                }
            }

            for (int h = y; h > y - 256; h--)
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
                
                for (int h = y; h > y - 256; h--)
                {
                    if (GetBlock(w, h).solid)
                    {
                        depth[index] = h;
                        break;
                    }
                }
            }

            for (int d = 0; d < depth.Length; d++)
            {
                if (d > 0)
                {
                    if (Math.Abs(depth[d] - depth[d - 1]) >= 10)
                    {
                        break;
                    }
                }
            }

            if (start_y >= 0)
            {
                
                
                for (int w = start_x; w < end_x; w++)
                {
                    for (int h = depth[w - start_x]; h < y; h++)
                    {
                        if (GetBlock(w, h) == BlockRegistry.air)
                        {
                            SetBlock(w, h, BlockRegistry.water);
                        }
                    }
                }
            }
        }

        generated = true;
        entities.Add(new Enemy(this));
        
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
        Chunk chunk = GetChunk(chunkX, chunkY);
        chunk.SetBlock(x, y, block);
        if (generated)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    MarkForUpdate(x + i, y + j);
                }
            }
        }
    }
    
    public void MarkForUpdate(int x, int y)
    {
        x &= 8191;
        y &= 8191;
        int chunkX = x / 128;
        int chunkY = y / 128;
        Chunk chunk = GetChunk(chunkX, chunkY);
        chunk.MarkForUpdate(x, y);
    }

    public Block GetBlock(int x, int y)
    {
        x &= 8191;
        y &= 8191;
        int chunkX = x / 128;
        int chunkY = y / 128;
        return chunks[chunkX + chunkY * 64].GetBlock(x, y);
        
    }
    
    public Vector4 GetColor(int x, int y)
    {
        x &= 8191;
        y &= 8191;
        int chunkX = x / 128;
        int chunkY = y / 128;
        return chunks[chunkX + chunkY * 64].GetBlock(x, y).GetColor(x, y);
        
    }

    public void Update()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Update();
            if (entities[i].position.Y < 0 && entities[i].can_fall_out_of_world || entities[i].health <= 0)
            {
                entities[i].Dispose();
                entities.RemoveAt(i);
            }
        }
        
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].Update();
        }

        physicsWorld.Step(1.0f / 60.0f, 6, 2);
    }
    
    public void Render()
    {
        Camera camera = Game.INSTANCE.camera;
        int cameraX = (int)MathF.Floor(camera.position.X/128);
        int cameraY = (int)MathF.Floor(camera.position.Y/128);
        
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Render();
        }
        
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].Render();
        }
    }

    //create chunk entity
    public void CreateChunkEntity(Vector2i searchPos, int searchSize, Vector2 position, Vector2 velocity, float duration, bool circle = false)
    {
        Vector4[] colors = new Vector4[searchSize * searchSize];
        bool empty = true;

        int start_x = -1;
        int start_y = -1;
        int end_x = -1;
        int end_y = -1;
            
        for (int x = 0; x < searchSize; x++)
        {
            for (int y = 0; y < searchSize; y++)
            {
                if (circle)
                {
                    float rad = MathF.Sqrt((x - searchSize / 2.0f) * (x - searchSize / 2.0f) +
                                        (y - searchSize / 2.0f) * (y - searchSize / 2.0f));
                    if (rad > searchSize / 2.0f)
                    {
                        continue;
                    }
                }
                if (!GetBlock(searchPos.X + x - searchSize / 2, searchPos.Y + y - searchSize / 2).can_break)
                {
                    continue;
                }
                colors[x + y * searchSize] = GetColor(searchPos.X + x - searchSize / 2, searchPos.Y + y - searchSize / 2);
                SetBlock(searchPos.X + x - searchSize / 2, searchPos.Y + y - searchSize / 2, BlockRegistry.air);
                if (colors[x + y * searchSize].W > 0)
                {
                    empty = false;
                    if (start_x == -1)
                    {
                        start_x = x;
                    }
                    else
                    {
                        start_x = (int)Math.Min(start_x, x);
                    }
                    if (start_y == -1)
                    {
                        start_y = y;
                    }else
                    {
                        start_y = (int)Math.Min(start_y, y);
                    }

                    end_x = Math.Max(end_x, x);
                    end_y = Math.Max(end_y, y);
                }
            }
        }

        int width = end_x - start_x;
        int height = end_y - start_y;

        if (width <= 0 || height <= 0 || start_x < 0 || start_y < 0 || end_x <= 0 || end_y <= 0) return;

        Vector4[] real_colors = new Vector4[width * height];
        for (int x = start_x; x < end_x; x++)
        {
            for (int y = start_y; y < end_y; y++)
            {
                real_colors[x - start_x + (y - start_y) * width] = colors[x + y * searchSize];
            }
        }

        if (!empty)
        {
            ChunkEntity chunk = new ChunkEntity(this, position, real_colors, width, height, duration);
            chunk.body.SetLinearVelocity(new System.Numerics.Vector2(velocity.X, velocity.Y));
            entities.Add(chunk);
        }
    }
    
    public void CreateParticleEntity(Vector2 position, Texture texture, int width, int height, float duration, int count, float posOffset, float sizeOffset, float growthDuration, float growthRate)
    {
        ParticleEntity explosion = new ParticleEntity(this, position, texture, width, height, duration, count, posOffset, sizeOffset, growthDuration, growthRate);
        entities.Add(explosion);
    }

    public void Dispose()
    {
        foreach (Chunk chunk in chunks)
        {
            chunk.chunkTexture.Dispose();
        }

        physicsWorld.Dispose();
    }
    
}