namespace Block
{
    using Blocks;

    public enum BlockType
    {
        Air,
        BirchLog,
        GrassBlock,
    }

    public struct BlockManager
    {
        // Attention: must be in the same order as in BlockType
        public static readonly BlockProperty[] BlockDict =
        {
            Air.Property,
            BirchLog.Property,
            GrassBlock.Property,
        };
    }
}