using UnityEngine;

public static class Block {
    private static readonly Tweaks Tweaks = Resources.Load<Tweaks>("ScriptableObjects/Tweaks");

    private static BlockType[,,] Blocks = new BlockType[Tweaks.maxWorldSize * Tweaks.chunkLength, Tweaks.chunkHeight, Tweaks.maxWorldSize * Tweaks.chunkLength];
    
    public static readonly RangeInt OfWood = new ((int) BlockType.AcaciaPlanks, 24);
    public static readonly RangeInt OfLeaf = new ((int) BlockType.AcaciaLeaves, 6);
    public static readonly RangeInt OfGlass = new ((int) BlockType.Glass, 17);
    public static readonly RangeInt CanPlant = new ((int) BlockType.Grass, 11);

    public static void SetBlock(Vector3 blockPos, BlockType blockType)
    {
        Blocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = blockType;
    }
    
    public static void SetBlock(int x, int y, int z, BlockType blockType)
    {
        Blocks[x, y, z] = blockType;
    }
    
    public static void SetBlock(int x, int y, int z, BlockType blockType, RangeInt blockMask)
    {
        if (blockMask.start < (int)Blocks[x, y, z] && (int)Blocks[x, y, z] < blockMask.end) return;
        Blocks[x, y, z] = blockType;
    }
    
    public static BlockType GetBlock(Vector3 blockPos) {
        return Blocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z];
    }
    
    public static BlockType GetBlock(int x, int y, int z) {
       return Blocks[x, y, z];
    }
    
    public static bool IsBlock(int x, int y, int z, BlockType blockType) {
        return Blocks[x, y, z] == blockType;
    }
    
    public static int GetTopBlockHeight(int x, int z) {
        for (int i = 5; i < Tweaks.chunkHeight; i++) {
            if(Blocks[x, i, z] != BlockType.Air)  continue;
            return i - 1;
        }
        return Tweaks.chunkHeight;
    }

    public static bool IsBlockInRange(BlockType blockType, RangeInt rangeInt) {
        return rangeInt.start <= (int) blockType && (int) blockType <= rangeInt.end;
    }
}

public enum BlockType
{
    Air,
    // grass & flower 
    Grass, TallGrass,
    LilyOfTheValley, Dandelion, Poppy, Allium, OxeyeDaisy, WhiteTulip, OrangeTulip, PinkTulip, Peony,
    
    GrassBlock, Dirt,
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