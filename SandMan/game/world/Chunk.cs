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

    public Chunk(BlockWorld world, int x, int y)
    {
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