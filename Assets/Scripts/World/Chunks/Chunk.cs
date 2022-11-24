using Unity.Mathematics;
using UnityEngine.Rendering;

namespace World.Chunks
{
    using System;
    using System.Collections.Generic;
    using Block;
    using Block.Blocks;
    using Render.Texture;
    using UnityEngine;
    using Utilities;

    [RequireComponent(typeof(MeshCollider), typeof(MeshFilter), typeof(MeshRenderer))]
    public class Chunk : MonoBehaviour
    {
        public const int Lenght = 16;
        public const int Width = 16;
        public const int Height = 256;
        public static readonly Vector3Int Size = new(Lenght, Height, Width);
        [HideInInspector] public Vector2Int id;
        public Block[,,] blockArray = new Block[Lenght + 2, Height, Width + 2];

        public int TopBlock { get; private set; }

        public int BottomBlock { get; private set; }

        /// <summary>
        /// the south-west corner where the y-axis is 0.
        /// <para> (minx, 0, minz)</para>
        /// </summary>
        public static Vector2Int Position { get; private set; }

        public Vector3Int TopBlockPos { get; private set; }

        public Vector3Int BottomBlockPos { get; private set; }

        private Mesh solidBlockMesh;
        private Mesh noColliderBlockMesh;

        private List<Vector3> vertices = new();
        private List<int> triangles = new();
        private List<Vector2> uvs = new();
        private int verticesCount;


        public void Gen(Vector2Int _id)
        {
            id = _id;
            SetBlock();

            
            
            solidBlockMesh = new Mesh();
            solidBlockMesh.indexFormat = IndexFormat.UInt32;    // using 32bit index buffer
            solidBlockMesh.MarkDynamic(); // Optimize mesh for frequent updates.

            
            Block currentBlock = new();

            for (int y = 0; y < Height; y++)
            {
                for (int z = 1; z < Width + 1; z++)
                {
                    for (int x = 1; x < Lenght + 1; x++)
                    {
                        currentBlock = blockArray[x, y, z];
                        if (currentBlock.TypeOf(BlockType.Air)) continue;
                        
                        int x0 = x - 1;
                        int z0 = z - 1;

                        if (currentBlock.VisualFace.all)
                        {
                            SetMesh(currentBlock, new Vector3(x0, y, z0));
                        }
                        else
                        {
                            if (blockArray[x + 1, y, z].VisualFace.left) SetMesh(currentBlock, BlockFace.Right, new Vector3(x0, y, z0));
                            if (blockArray[x - 1, y, z].VisualFace.right) SetMesh(currentBlock, BlockFace.Left, new Vector3(x0, y, z0));
                            if (y == Height - 1 || blockArray[x, y + 1, z].VisualFace.down) SetMesh(currentBlock, BlockFace.Top, new Vector3(x0, y, z0));
                            if (y == 0 || blockArray[x, y - 1, z].VisualFace.top) SetMesh(currentBlock, BlockFace.Down, new Vector3(x0, y, z0));
                            if (blockArray[x, y, z + 1].VisualFace.back) SetMesh(currentBlock, BlockFace.Front, new Vector3(x0, y, z0));
                            if (blockArray[x, y, z - 1].VisualFace.front) SetMesh(currentBlock, BlockFace.Back, new Vector3(x0, y, z0));
                        }
                    }
                }
            }

            Debug.Log(vertices.Count);
            Debug.Log(triangles.Count);
            Debug.Log(uvs.Count);

            solidBlockMesh.vertices = vertices.ToArray();
            solidBlockMesh.triangles = triangles.ToArray();
            solidBlockMesh.uv = uvs.ToArray();

            SetComponent();
        }

        private void SetBlock()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Width + 2; z++)
                {
                    for (int x = 0; x < Lenght + 2; x++)
                    {
                        if (x is 0 or Lenght + 1 || z is 0 or Width + 1)
                        {
                            blockArray[x, y, z] = new Air();
                            continue;
                        }
                        blockArray[x, y, z] = new GrassBlock();
                    }
                }
            }
        }

        // private int SimplexNoise(int _x, int _z)
        // {
        // return noise.snoise()
        // }

        private void SetMesh(Block _block, Vector3 _pos)
        {
            vertices.AddRange(_block.GetAllVertices(_pos));
            verticesCount += 24;
            triangles.AddRange(_block.GetAllTriangles(verticesCount));
            uvs.AddRange(_block.GetAllUVs());
        }

        private void SetMesh(Block _block, BlockFace _face, Vector3 _pos)
        {
            vertices.AddRange(_block.GetVertices(_face, _pos));
            verticesCount += 4;
            triangles.AddRange(_block.GetTriangles(verticesCount));
            uvs.AddRange(_block.GetUVs(_face));
        }

        private void SetComponent()
        {
            GetComponent<MeshRenderer>().sharedMaterial = new Material(GameAssets.MainBlockMaterial)
            {
                mainTexture = BlockTexture.MainTexture
            };
            solidBlockMesh.RecalculateNormals();
            GetComponent<MeshFilter>().sharedMesh = solidBlockMesh;

            GetComponent<MeshCollider>().sharedMesh = solidBlockMesh;
            // GetComponents<MeshCollider>()[1].sharedMesh = noColliderBlockMesh;
        }
    }
}