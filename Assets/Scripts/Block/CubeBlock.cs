namespace Block
{
    using System;
    using System.Linq;
    using UnityEngine;
    using Render.Mesh;
    using Render.Texture;

    public class CubeBlock : Block
    {
        public override bool VisualAllFace => false;

        #region Mesh

        private static readonly Vertices Right = new() { array = new Vector3[] { new(1, 1, 1), new(1, 1, 0), new(1, 0, 1), new(1, 0, 0) } };
        private static readonly Vertices Left = new() { array = new Vector3[] { new(0, 1, 0), new(0, 1, 1), new(0, 0, 0), new(0, 0, 1) } };
        private static readonly Vertices Top = new() { array = new Vector3[] { new(1, 1, 1), new(0, 1, 1), new(1, 1, 0), new(0, 1, 0) } };
        private static readonly Vertices Down = new() { array = new Vector3[] { new(0, 0, 1), new(1, 0, 1), new(0, 0, 0), new(1, 0, 0) } };
        private static readonly Vertices Front = new() { array = new Vector3[] { new(0, 1, 1), new(1, 1, 1), new(0, 0, 1), new(1, 0, 1) } };
        private static readonly Vertices Back = new() { array = new Vector3[] { new(1, 1, 0), new(0, 1, 0), new(1, 0, 0), new(0, 0, 0) } };

        public override Vector3[] GetVertices(BlockFace _blockFace, Vector3 _pos) => _blockFace switch
        {
            BlockFace.Right => Right + _pos,
            BlockFace.Left => Left + _pos,
            BlockFace.Top => Top + _pos,
            BlockFace.Down => Down + _pos,
            BlockFace.Front => Front + _pos,
            BlockFace.Back => Back + _pos,
            _ => throw new ArgumentOutOfRangeException(nameof(_blockFace), _blockFace, null)
        };


        public override Vector3[] GetAllVertices(Vector3 _pos)
        {
            return (Right + _pos).Concat(Left + _pos).Concat(Top + _pos).Concat(Down + _pos).Concat(Front + _pos).Concat(Back + _pos).ToArray();
        }

        public override int[] GetTriangles(int _vertices)
        {
            _vertices -= 4;
            return new[] { _vertices, _vertices + 3, _vertices + 1, _vertices, _vertices + 2, _vertices + 3 };
        }

        public override int[] GetAllTriangles(int _vertices)
        {
            _vertices -= 24;
            int[] tris = new int[36];
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

        public override Vector2[] GetUVs(BlockFace _face)
        {
            UVPos uv = BlockTexture.GetUVPos(this, _face);

            return new Vector2[]
            {
                new(uv.endPoint.x - TrimmingDistance, uv.endPoint.y - TrimmingDistance),
                new(uv.startPoint.x + TrimmingDistance, uv.endPoint.y - TrimmingDistance),
                new(uv.endPoint.x - TrimmingDistance, uv.startPoint.y + TrimmingDistance),
                new(uv.startPoint.x + TrimmingDistance, uv.startPoint.y + TrimmingDistance)
            };
        }

        public override Vector2[] GetAllUVs()
        {
            return GetUVs(BlockFace.Right)
                .Concat(GetUVs(BlockFace.Left))
                .Concat(GetUVs(BlockFace.Top))
                .Concat(GetUVs(BlockFace.Down))
                .Concat(GetUVs(BlockFace.Front))
                .Concat(GetUVs(BlockFace.Back)).ToArray();
        }

        #endregion
    }
}