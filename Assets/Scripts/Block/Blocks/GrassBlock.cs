using Render.Mesh;

namespace Block.Blocks
{
    public class GrassBlock : Block
    {
        public static readonly BlockProperty Property = new() { name = "grass_block", type = BlockType.GrassBlock, mesh = BlockMesh.CubeBlock, visibleFace = VisibleFace.NoFace };
        
    }
}