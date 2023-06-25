using OpenTK.Mathematics;
using SandMan.game.world;

namespace SandMan.game.entities;

public class Entity
{
    public Vector2 position;
    public Vector2 motion;

    public World world;

    public Entity(World world)
    {
        this.world = world;
    }
    
    public virtual void Update()
    {
        
    }

    public virtual void Render()
    {
        
    }
}