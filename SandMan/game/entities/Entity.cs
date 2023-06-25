using OpenTK.Mathematics;
using SandMan.game.world;

namespace SandMan.game.entities;

public class Entity
{
    public Vector2 position;
    public Vector2 motion;

    public BlockWorld world;
    public bool can_fall_out_of_world = false;

    public Entity(BlockWorld world, Vector2 position)
    {
        this.position = position;
        this.world = world;
    }
    
    public virtual void Update()
    {
        
    }

    public virtual void Render()
    {
        
    }

    public virtual void Dispose()
    {
        
    }
}