using OpenTK.Mathematics;
using SandMan.rendering;

namespace SandMan.game;

public class LevelGeneration
{
    private Texture world;

    public LevelGeneration()
    {
        world = new Texture(8000, 1000, new Vector4(0f, 0f, 1f, 1f));
    }

    public void GenerateLevel()
    {
        
    }
    
}