using UnityEngine;

public static class Block {
    private static readonly Tweaks Tweaks = Resources.Load<Tweaks>("ScriptableObjects/Tweaks");

    private static BlockType[,,] Blocks = new BlockType[Tweaks.maxWorldSize * Tweaks.chunkLength, Tweaks.chunkHeight, Tweaks.maxWorldSize * Tweaks.chunkLength];
    
    private static readonly RangeInt IsWood = new ((int) BlockType.AcaciaLog, (int) BlockType.StrippedSpruceWood);
    private static readonly RangeInt IsLeaf = new ((int) BlockType.AcaciaLeaves, (int) BlockType.SpruceLeaves);

    public static void SetBlock(int x, int y, int z, BlockType blockType)
    {
        Blocks[x, y, z] = blockType;
    }
    
    public static void SetBlock(int x, int y, int z, BlockType blockType, RangeInt blockMask)
    {
        if (blockMask.start < (int)Blocks[x, y, z] && (int)Blocks[x, y, z] < blockMask.end) return;
        Blocks[x, y, z] = blockType;
    }
    
    public static BlockType GetBlock(int x, int y, int z) {
       return Blocks[x, y, z];
    }
    
    public static bool IsBlock(int x, int y, int z, BlockType blockType) {
        return Blocks[x, y, z] == blockType;
    }
    
    public static int GetTopBlockHeight(int x, int z) {
        for (int i = 0; i < Tweaks.chunkHeight; i++) {
            if(Blocks[x, i, z] != BlockType.Air)  continue;
            return i - 1;
        }
        return Tweaks.chunkHeight;
    }
}

public enum BlockType
{
    Air,
    GrassBlock, Dirt,
    Grass, TallGrass,
    // flower
    LilyOfTheValley, Dandelion, Poppy, Allium, OxeyeDaisy, WhiteTulip, OrangeTulip, PinkTulip, Peony,
    // stone
    Cobblestone, Stone, SmoothStone, StoneBricks, CrackedStoneBricks, ChiseledStoneBricks,
    Bedrock, Obsidian, Gravel, Andesite, PolishedAndesite, Diorite, PolishedDiorite, Granite, PolishedGranite, Bricks,
    // ore
    CoalOre, IronOre, DiamondOre, GoldOre, RedstoneOre, CoalBlock, IronBlock, DiamondBlock, GoldBlock, RedstoneBlock,
    Sand,
    // wood
    AcaciaPlanks, BirchPlanks, DarkOakPlanks, JunglePlanks, OakPlanks, SprucePlanks,
    AcaciaLog, BirchLog, DarkOakLog, JungleLog, OakLog, SpruceLog,
    StrippedAcaciaLog, StrippedBirchLog, StrippedDarkOakLog, StrippedJungleLog, StrippedOakLog, StrippedSpruceLog,
    StrippedAcaciaWood, StrippedBirchWood, StrippedDarkOakWood, StrippedJungleWood, StrippedOakWood, StrippedSpruceWood,
    AcaciaLeaves, BirchLeaves, DarkOakLeaves, JungleLeaves, OakLeaves, SpruceLeaves,
    // glass
    Glass,
    BlackStainedGlass, BlueStainedGlass, BrownStainedGlass, CyanStainedGlass, GrayStainedGlass, GreenStainedGlass, 
    LightBlueStainedGlass, LightGrayStainedGlass, LimeStainedGlass, MagentaStainedGlass,OrangeStainedGlass, 
    PinkStainedGlass, PurpleStainedGlass, RedStainedGlass, WhiteStainedGlass, YellowStainedGlass, 
    // quartz
    QuartzBlock, SmoothQuartzBlock, QuartzPillar, ChiseledQuartzBlock, QuartzBricks
}

public enum Direction
{
   Top, Down, Front, Back, Right, Left
}