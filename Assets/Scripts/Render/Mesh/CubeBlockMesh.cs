namespace Render.Mesh
{
    using System;
    using System.Linq;
    using Block;
    using Texture;
    using UnityEngine;

    public class CubeBlockMesh : BlockMesh
    {
        public override int AllVerticesCount => 24;
        public override int AllTrianglesCount => 36;
        public override int AllUVsCount => 24;


        #region Mesh

        private static readonly Vector3[] Right = { new(1, 1, 1), new(1, 1, 0), new(1, 0, 1), new(1, 0, 0) };
        private static readonly Vector3[] Left = { new(0, 1, 0), new(0, 1, 1), new(0, 0, 0), new(0, 0, 1) };
        private static readonly Vector3[] Top = { new(1, 1, 1), new(0, 1, 1), new(1, 1, 0), new(0, 1, 0) };
        private static readonly Vector3[] Down = { new(0, 0, 1), new(1, 0, 1), new(0, 0, 0), new(1, 0, 0) };
        private static readonly Vector3[] Front = { new(0, 1, 1), new(1, 1, 1), new(0, 0, 1), new(1, 0, 1) };
        private static readonly Vector3[] Back = { new(1, 1, 0), new(0, 1, 0), new(1, 0, 0), new(0, 0, 0) };

        private readonly Vector3[] c = new Vector3[4];

        public override Vector3[] GetVertices(BlockFace _blockFace, int _x, int _y, int _z)
        {
            switch (_blockFace)
            {
                case BlockFace.Back:
                    c[0].x = _x + 1;
                    c[0].y = _y + 1;
                    c[0].z = _z;
                    c[1].x = _x;
                    c[1].y = _y + 1;
                    c[1].z = _z;
                    c[2].x = _x + 1;
                    c[2].y = _y;
                    c[2].z = _z;
                    c[3].x = _x;
                    c[3].y = _y;
                    c[3].z = _z;
                    return c;
                case BlockFace.Down:
                    c[0].x = _x;
                    c[0].y = _y;
                    c[0].z = _z + 1;
                    c[1].x = _x + 1;
                    c[1].y = _y;
                    c[1].z = _z + 1;
                    c[2].x = _x;
                    c[2].y = _y;
                    c[2].z = _z;
                    c[3].x = _x + 1;
                    c[3].y = _y;
                    c[3].z = _z;
                    return c;
                case BlockFace.Front:
                    c[0].x = _x;
                    c[0].y = _y + 1;
                    c[0].z = _z + 1;
                    c[1].x = _x + 1;
                    c[1].y = _y + 1;
                    c[1].z = _z + 1;
                    c[2].x = _x;
                    c[2].y = _y;
                    c[2].z = _z + 1;
                    c[3].x = _x + 1;
                    c[3].y = _y;
                    c[3].z = _z + 1;
                    return c;
                case BlockFace.Left:
                    c[0].x = _x;
                    c[0].y = _y + 1;
                    c[0].z = _z;
                    c[1].x = _x;
                    c[1].y = _y + 1;
                    c[1].z = _z + 1;
                    c[2].x = _x;
                    c[2].y = _y;
                    c[2].z = _z;
                    c[3].x = _x;
                    c[3].y = _y;
                    c[3].z = _z + 1;
                    return c;
                case BlockFace.Right:
                    c[0].x = _x + 1;
                    c[0].y = _y + 1;
                    c[0].z = _z + 1;
                    c[1].x = _x + 1;
                    c[1].y = _y + 1;
                    c[1].z = _z;
                    c[2].x = _x + 1;
                    c[2].y = _y;
                    c[2].z = _z + 1;
                    c[3].x = _x + 1;
                    c[3].y = _y;
                    c[3].z = _z;
                    return c;
                case BlockFace.Top:
                    c[0].x = _x + 1;
                    c[0].y = _y + 1;
                    c[0].z = _z + 1;
                    c[1].x = _x;
                    c[1].y = _y + 1;
                    c[1].z = _z + 1;
                    c[2].x = _x + 1;
                    c[2].y = _y + 1;
                    c[2].z = _z;
                    c[3].x = _x;
                    c[3].y = _y + 1;
                    c[3].z = _z;
                    return c;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_blockFace), _blockFace, null);
            }
        }

        private Vector3[] Add(Vector3[] _vs, int _x, int _y, int _z)
        {
            c[0].x = _vs[0].x + _x;
            c[1].x = _vs[1].x + _x;
            c[2].x = _vs[2].x + _x;
            c[3].x = _vs[3].x + _x;

            c[0].y = _vs[0].y + _y;
            c[1].y = _vs[1].y + _y;
            c[2].y = _vs[2].y + _y;
            c[3].y = _vs[3].y + _y;

            c[0].z = _vs[0].z + _z;
            c[1].z = _vs[1].z + _z;
            c[2].z = _vs[2].z + _z;
            c[3].z = _vs[3].z + _z;

            return c;
        }

        public override Vector3[] GetAllVertices(int _x, int _y, int _z)
        {
            return Add(Right, _x, _y, _z)
                .Concat(Add(Left, _x, _y, _z))
                .Concat(Add(Top, _x, _y, _z))
                .Concat(Add(Down, _x, _y, _z))
                .Concat(Add(Front, _x, _y, _z))
                .Concat(Add(Back, _x, _y, _z)).ToArray();
        }

        private readonly int[] tri = new int[6];

        public override int[] GetTriangles(int _vertices)
        {
            _vertices -= 4;
            tri[0] = _vertices;
            tri[1] = _vertices + 3;
            tri[2] = _vertices + 1;
            tri[3] = _vertices;
            tri[4] = _vertices + 2;
            tri[5] = _vertices + 3;
            return tri;
        }

        private readonly int[] tris = new int[36];

        public override int[] GetAllTriangles(int _vertices)
        {
            _vertices -= 24;
            for (int i = 0; i < 6; i++)
            {
                tris[i * 6] = _vertices + 4 * i;
                tris[i * 6 + 1] = _vertices + 4 * i + 3;
                tris[i * 6 + 2] = _vertices + 4 * i + 1;
                tris[i * 6 + 3] = _vertices + 4 * i;
                tris[i * 6 + 4] = _vertices + 4 * i + 2;
                tris[i * 6 + 5] = _vertices + 4 * i + 3;
            }

            return tris.ToArray();
        }

        private readonly Vector2[] uvPos = new Vector2[4];

        public override Vector2[] GetUVs(BlockProperty _block, BlockFace _face)
        {
            UVPos uv = BlockTexture.UVPosArray[BlockTexture.BlockTextureIndex[(int)_block.type - 1, (int)_face]];

            uvPos[0].x = uv.x1 - TrimmingDistance;
            uvPos[0].y = uv.y1 - TrimmingDistance;
            uvPos[1].x = uv.x0 + TrimmingDistance;
            uvPos[1].y = uv.y1 - TrimmingDistance;
            uvPos[2].x = uv.x1 - TrimmingDistance;
            uvPos[2].y = uv.y0 + TrimmingDistance;
            uvPos[3].x = uv.x0 + TrimmingDistance;
            uvPos[3].y = uv.y0 + TrimmingDistance;

            return uvPos;
        }

        public override Vector2[] GetAllUVs(BlockProperty _block)
        {
            return GetUVs(_block, BlockFace.Right)
                .Concat(GetUVs(_block, BlockFace.Left))
                .Concat(GetUVs(_block, BlockFace.Top))
                .Concat(GetUVs(_block, BlockFace.Down))
                .Concat(GetUVs(_block, BlockFace.Front))
                .Concat(GetUVs(_block, BlockFace.Back)).ToArray();
        }

        #endregion
    }
}