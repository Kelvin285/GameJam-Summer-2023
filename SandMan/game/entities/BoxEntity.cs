using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using OpenTK.Mathematics;
using SandMan.game.world;

namespace SandMan.game.entities;

public class BoxEntity : Entity
{
    public Body body;

    public int width, height;
    public BoxEntity(BlockWorld blockWorld, Vector2 position) : base(blockWorld, position)
    {
        this.width = 10;
        this.height = 10;
        
        BodyDef def = new BodyDef();
        def.Position = new(position.X, position.Y);
        def.BodyType = BodyType.DynamicBody;
        
        this.body = world.physicsWorld.CreateBody(def);
        
        PolygonShape shape = new PolygonShape();
        shape.SetAsBox(width, height);
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

        Game.DrawTexture(Game.INSTANCE.texture, new Vector2(pos.X, pos.Y), new Vector2(10, 10) * 2,MathHelper.RadiansToDegrees(rotation), true);
    }

    public override void Update()
    {
        base.Update();
        
        position = new Vector2(body.GetPosition().X, body.GetPosition().Y);

        if (world.GetBlock((int)position.X, (int)position.Y).solid)
        {
            body.SetTransform(body.GetPosition() + new System.Numerics.Vector2(0, height / 2.0f), body.GetAngle());
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        world.physicsWorld.DestroyBody(body);
    }
}