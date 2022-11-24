using System;
using Block.Blocks;
using UnityEngine;

namespace Block
{
    using Utilities;
    using Render.Mesh;

    public class Block : BlockMesh
    {
        public virtual int Length { get; } = 1;
        public virtual int Width { get; } = 1;
        public virtual int Height { get; } = 1;

        public BlockType Type => Enum.Parse<BlockType>(TypeName);
        public string TypeName => GetType().Name;
        public string Name => StringTool.LowercaseWithUnderline(TypeName);
        public string TextureName() => $"{StringTool.LowercaseWithUnderline(Name)}";
        public string TextureName(BlockFace _face) => $"{StringTool.LowercaseWithUnderline(Name)}_{_face.ToString().ToLower()}";

        public bool TypeOf(BlockType _type) => Type == _type;
        
    }
}