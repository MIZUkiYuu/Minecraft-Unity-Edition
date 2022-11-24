using Render.Mesh;

namespace Block.Blocks
{
    public class BirchLog : Block
    {
        public static readonly BlockProperty Property = new() { name = "birch_log", type = BlockType.BirchLog, mesh = BlockMesh.CubeBlock, visibleFace = VisibleFace.NoFace };
    }
}