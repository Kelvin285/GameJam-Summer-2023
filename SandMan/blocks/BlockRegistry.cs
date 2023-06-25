using OpenTK.Mathematics;

namespace SandMan.blocks;

public class BlockRegistry
{
    public static List<Block> blocks = new();

    public static Block air = new Block(new Vector4(0f, 0f, 0f, 0f), false);
    public static Block sand = new Block("sand.png", can_break: true);
    public static Block water = new WaterBlock(new Vector4(0f, 0f, 1, 0.5f), false);

}