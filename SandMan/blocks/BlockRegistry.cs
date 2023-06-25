using OpenTK.Mathematics;

namespace SandMan.blocks;

public class BlockRegistry
{
    public static List<Block> blocks = new();

    public static Block air = new Block(new Vector4(0f, 0f, 0f, 0f) / 255f, false);
    public static Block sand = new Block("sand.png");
    public static Block water = new Block(new Vector4(0f, 0f, 255f, 255f) / 255f, false);

}