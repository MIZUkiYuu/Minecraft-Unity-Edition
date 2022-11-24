using Render.Mesh;

namespace Block.Blocks
{
    public class Air : Block
    {
        public static readonly BlockProperty Property = new() { name = "air", type = BlockType.Air, mesh = BlockMesh.None, visibleFace = VisibleFace.AllFace };
    }
}