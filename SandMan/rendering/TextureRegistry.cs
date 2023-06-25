namespace SandMan.rendering;

public class TextureRegistry
{
    public static List<Texture> textures = new List<Texture>();

    public static Texture PLAYER_HEAD;
    public static Texture PLAYER_CANNON;
    public static Texture PLAYER_LEG_CONNECTION;
    public static Texture PLAYER_LEG_UP;
    public static Texture PLAYER_LEG_DOWN;
    
    public static Texture ENEMY;
    public static Texture SHARK;

    public static Texture EXPLOSION;
    

    public static Texture AddTexture(string path)
    {
        Texture texture = new Texture(path);
        textures.Add(texture);
        return texture;
    }
    
    public static void RegisterTextures()
    {
        PLAYER_HEAD = AddTexture("assets/textures/player/head.png");
        PLAYER_LEG_CONNECTION = AddTexture("assets/textures/player/leg_attachment.png");
        PLAYER_LEG_UP = AddTexture("assets/textures/player/leg_up.png");
        PLAYER_LEG_DOWN = AddTexture("assets/textures/player/leg_down.png");
        PLAYER_CANNON = AddTexture("assets/textures/player/cannon.png");
        
        ENEMY = AddTexture("assets/textures/enemy/enemy.png");
        SHARK = AddTexture("assets/textures/enemy/shark.png");

        EXPLOSION = AddTexture("assets/textures/explosion/explosion.png");
    }

    public static void Dispose()
    {
        foreach (Texture t in textures) t.Dispose();
    }
}