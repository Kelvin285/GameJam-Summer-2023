using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using OpenTK.Mathematics;
using SandMan.blocks;
using SandMan.game.world;
using SandMan.rendering;

namespace SandMan.game.entities;

public class ChunkEntity : Entity
{
    public Body body;

    public Texture texture;
    
    public int width, height;
    public ChunkEntity(BlockWorld blockWorld, Vector2 position, Vector4[] colors, int width, int height) : base(blockWorld, position)
    {
        this.width = width;
        this.height = height;
        texture = new Texture(width, height, Vector4.Zero);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, colors[x + y * width]);
            }
        }
        texture.Update();
        
        BodyDef def = new BodyDef();
        def.Position = new(position.X, position.Y);
        def.BodyType = BodyType.DynamicBody;
        
        this.body = world.physicsWorld.CreateBody(def);
        
        PolygonShape shape = new PolygonShape();
        shape.SetAsBox(width / 2.0f, height / 2.0f);
        shape.Validate();

        FixtureDef fixture = new FixtureDef();
        fixture.Density = 1.0f;
        fixture.Shape = shape;
        fixture.Friction = 0.3f;
        
        body.CreateFixture(fixture);
        
        can_fall_out_of_world = true;
    }

    public override void Render()
    {
        base.Render();

        var pos = body.GetPosition();
        var rotation = body.GetAngle();

        Game.DrawTexture(texture, new Vector2(pos.X, pos.Y), new Vector2(width, height),MathHelper.RadiansToDegrees(rotation), true);
    }

    public override void Update()
    {
        base.Update();
        
        position = new Vector2(body.GetPosition().X, body.GetPosition().Y);
    }

    public override void Dispose()
    {
        base.Dispose();
        world.physicsWorld.DestroyBody(body);
        texture.Dispose();
    }
}