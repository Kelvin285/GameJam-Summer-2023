using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
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

    public Body body;
    public bool has_body = false;

    public BlockWorld world;

    public class UpdateRect
    {
        public int x1;
        public int y1;
        public int x2;
        public int y2;
    };

    public int update_timer = 0;

    public UpdateRect updateRect = new UpdateRect();

    public Chunk(BlockWorld world, int x, int y)
    {
        updateRect.x1 = 500;
        updateRect.y1 = 500;
        updateRect.x2 = -500;
        updateRect.y2 = -500;
        this.world = world;
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

    public void MarkForUpdate(int x, int y)
    {
        x &= 127;
        y &= 127;
        if (x > updateRect.x2)
        {
            updateRect.x2 = x + 1;
            if (updateRect.x2 > 127) updateRect.x2 = 127;
        }

        if (x < updateRect.x1)
        {
            updateRect.x1 = x - 1;
            if (updateRect.x1 < 0) updateRect.x1 = 0;
        }
            
        if (y > updateRect.y2)
        {
            updateRect.y2 = y + 1;
            if (updateRect.y2 > 127) updateRect.y2 = 127;
        }

        if (y < updateRect.y1)
        {
            updateRect.y1 = y - 1;
            if (updateRect.y1 < 0) updateRect.y1 = 0;
        }
        
        update_timer = 1;
    }
    
    public Block GetBlock(int x, int y)
    {
        return BlockRegistry.blocks[blocks[(x & 127) + (y & 127) * 128]];
    }

    public void Update()
    {
        if (update_timer > 0)
        {
            int updates = 0;
            for (int y = updateRect.y1; y <= updateRect.y2; y++)
            {
                for (int x = updateRect.x1; x <= updateRect.x2; x++)
                {
                    var block = GetBlock(x, y);
                    if (block.Update(x + this.x * 128, y + this.y * 128, world))
                    {
                        updates++;
                    }
                }
            }

            if (updates == 0)
            {
                update_timer--;
            }
        }
        else
        {
            
            updateRect.x1 = 500;
            updateRect.y1 = 500;
            updateRect.x2 = -500;
            updateRect.y2 = -500;
        }
    }
    
    public void Render()
    {
        if (updated)
        {
            updated = false;
            chunkTexture.Update();

            if (has_body)
            {
                world.physicsWorld.DestroyBody(body);
            }

            has_body = true;
            
            BodyDef def = new BodyDef();
            def.Position = new(x * 128, y * 128);
            def.BodyType = BodyType.StaticBody;
        
            body = world.physicsWorld.CreateBody(def);
            
            for (int x = 0; x < 128; x++)
            {
                for (int y = 0; y < 128; y++)
                {
                    if (GetBlock(x, y).solid)
                    {
                        int X = x + this.x * 128;
                        int Y = y + this.y * 128;
                        if (world.GetBlock(X, Y + 1).solid == false || world.GetBlock(X, Y - 1).solid == false || world.GetBlock(X - 1, Y).solid == false || world.GetBlock(X + 1, Y).solid == false)
                        {
                            PolygonShape shape = new PolygonShape();
                            shape.SetAsBox(0.5f, 0.5f, new(x + 0.5f, y + 0.5f), 0);
                            shape.Validate();
                            
                            FixtureDef fixture = new FixtureDef();
                            fixture.Density = 1.0f;
                            fixture.Shape = shape;
                            fixture.Friction = 0.3f;
                            body.CreateFixture(fixture);
                        }
                    }
                }
            }
        }
        Game.DrawTexture(chunkTexture, new Vector2(x, y)*128, new Vector2(128, 128));
    }
    
}