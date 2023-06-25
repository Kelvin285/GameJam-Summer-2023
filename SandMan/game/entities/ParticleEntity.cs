using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using OpenTK.Mathematics;
using SandMan.game.world;
using SandMan.rendering;

namespace SandMan.game.entities;

public class ParticleEntity : Entity
{
    private Texture texture;
    private float timer;
    private float duration = 3f;
    private float growthDuration;
    private float growthRate;
    private List<List<float>> particles = new List<List<float>>();

    public float width, height;
    public int count;
    public float posOffset, sizeOffset;
    public ParticleEntity(BlockWorld blockWorld, Vector2 position, Texture texture, float width, float height, float duration, int count, float posOffset, float sizeOffset, float growthDuration = 0f, float growthRate = 0f) : base(blockWorld, position)
    {
        this.width = width;
        this.height = height;
        this.count = count;
        
        this.posOffset = posOffset;
        this.sizeOffset = sizeOffset;

        this.growthDuration = growthDuration;
        this.growthRate = growthRate;
        
        this.duration = duration;
        this.timer = 0f;

        this.texture = texture;

        for (int i = 0; i < count; i++)
        {
            float dx = (float)world.random.NextDouble()*2*posOffset - posOffset;
            float dy = (float)world.random.NextDouble()*2*posOffset - posOffset;
            float soff = (float)world.random.NextDouble() * 2 * sizeOffset - sizeOffset;
            float rotation = (float)world.random.Next(360);
            particles.Add( new List<float>() {dx, dy, soff, rotation} );
        }
    }

    public override void Render()
    {
        base.Render();

        width += 0.02f;
        height += 0.02f;

        for (int i = 0; i < count; i++)
        {
            float dx = particles[i][0];
            float dy = particles[i][1];
            float soff = particles[i][1];
            int rotation = (int)particles[i][3];
            Game.DrawTexture(texture, new Vector2(position.X+dx, position.Y+dy), new Vector2(width-soff, height-soff),MathHelper.RadiansToDegrees(rotation), true);
        }
    }

    public override void Update()
    {
        base.Update();

        timer += Game.INSTANCE.Delta;
        if (timer > duration)
        {
            health = 0;
        }
        if (timer <= growthDuration)
        {
            width += growthRate*Game.INSTANCE.Delta;
            height += growthRate*Game.INSTANCE.Delta;
        }
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}