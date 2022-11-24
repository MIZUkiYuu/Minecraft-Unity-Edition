namespace World.Chunks
{
    using System;
    using System.Buffers;
    using UnityEngine;
    using UnityEngine.Rendering;
    using Unity.Burst;
    using Render.Texture;
    using Block;

    public class ChunkGenerator
    {
        public ChunkGenerator()
        {
            vertices = verticesPool.Rent(32768);
            // triangles = trianglesPool.Rent(49152);
            uvs = uvsPool.Rent(32768);
        }

        ~ChunkGenerator()
        {
            verticesPool.Return(vertices);
            // trianglesPool.Return(triangles);
            uvsPool.Return(uvs);
        }

        private const int Length = Chunk.Length;
        private const int Height = Chunk.Height;
        private const int Width = Chunk.Width;

        private const int ExLength = Length + 2;
        private const int ExWidth = Width + 2;
        private const int PlaneArea = ExLength * ExWidth;

        private readonly BlockType[] blockArray = new BlockType[ExLength * Height * ExWidth];

        // create array pool to allocate space
        private ArrayPool<Vector3> verticesPool = ArrayPool<Vector3>.Shared;
        private ArrayPool<int> trianglesPool = ArrayPool<int>.Shared;
        private ArrayPool<Vector2> uvsPool = ArrayPool<Vector2>.Shared;

        // store all mesh data
        private Vector3[] vertices;
        private int[] triangles = new int[49152];
        private Vector2[] uvs;

        // store last block mesh data
        private Vector3[] currentVertices;
        private Vector2[] currentUVs;
        private int[] currentTriangles;

        private BlockType currentBlockType;
        private BlockProperty currentBlockProperty;

        // temporary
        private int i;
        private int x0;
        private int z0;
        private int zCount;
        private int yCount;
        private int xyCount;
        private int xzArea;
        private int yVolume;

        // using 32bit index buffer
        public readonly Mesh colliderMesh = new() { indexFormat = IndexFormat.UInt32 };
        public readonly Mesh nonColliderMesh = new() { indexFormat = IndexFormat.UInt32 };

        public void Gen(Chunk _chunk, Vector2Int _id)
        {
            _chunk.id = _id;

            vIndex = 0;
            tIndex = 0;
            uvIndex = 0;

            FillBlockData(_id);

            // Optimize mesh for frequent updates.
            colliderMesh.MarkDynamic();
            nonColliderMesh.MarkDynamic();

            for (int y = 0; y < Height; y++)
            {
                yVolume = y * PlaneArea;

                for (int z = 1; z < Width + 1; z++)
                {
                    zCount = z * ExLength;
                    yCount = zCount + yVolume;

                    for (int x = 1; x < Length + 1; x++)
                    {
                        currentBlockType = blockArray[x + yCount];
                        if (currentBlockType == BlockType.Air) continue;

                        currentBlockProperty = BlockManager.BlockDict[(int)currentBlockType];

                        x0 = x - 1;
                        z0 = z - 1;

                        // ensure array capacity
                        if (vIndex + 24 > vertices.Length)
                        {
                            Vector3[] v = verticesPool.Rent(vertices.Length * 2);
                            Array.ConstrainedCopy(vertices, 0, v, 0, vertices.Length);
                            vertices = v;
                            verticesPool.Return(v);

                            // int[] t = trianglesPool.Rent(triangles.Length * 2);
                            int[] t = new int[triangles.Length * 2];
                            Array.ConstrainedCopy(triangles, 0, t, 0, triangles.Length);
                            triangles = t;
                            // trianglesPool.Return(t);

                            Vector2[] uv = uvsPool.Rent(uvs.Length * 2);
                            Array.ConstrainedCopy(uvs, 0, uv, 0, uvs.Length);
                            uvs = uv;
                            uvsPool.Return(uv);
                        }


                        // set mesh data
                        if (currentBlockProperty.visibleFace.all)
                        {
                            SetAllMesh(currentBlockProperty, x0, y, z0);
                        }
                        else
                        {
                            xzArea = x + zCount;
                            xyCount = x + yVolume;

                            if (currentBlockProperty.visibleFace.right || BlockManager.BlockDict[(int)blockArray[x + 1 + yCount]].visibleFace.left)
                            {
                                SetMesh(currentBlockProperty, BlockFace.Right, x0, y, z0);
                            }

                            if (currentBlockProperty.visibleFace.left || BlockManager.BlockDict[(int)blockArray[x0 + yCount]].visibleFace.right)
                            {
                                SetMesh(currentBlockProperty, BlockFace.Left, x0, y, z0);
                            }

                            if (y == Height - 1 || currentBlockProperty.visibleFace.top || BlockManager.BlockDict[(int)blockArray[xzArea + (y + 1) * PlaneArea]].visibleFace.down)
                            {
                                SetMesh(currentBlockProperty, BlockFace.Top, x0, y, z0);
                            }

                            if (y == 0 || currentBlockProperty.visibleFace.down || BlockManager.BlockDict[(int)blockArray[xzArea + (y - 1) * PlaneArea]].visibleFace.top)
                            {
                                SetMesh(currentBlockProperty, BlockFace.Down, x0, y, z0);
                            }

                            if (currentBlockProperty.visibleFace.front || BlockManager.BlockDict[(int)blockArray[xyCount + (z + 1) * ExLength]].visibleFace.back)
                            {
                                SetMesh(currentBlockProperty, BlockFace.Front, x0, y, z0);
                            }

                            if (currentBlockProperty.visibleFace.back || BlockManager.BlockDict[(int)blockArray[xyCount + (z - 1) * ExLength]].visibleFace.front)
                            {
                                SetMesh(currentBlockProperty, BlockFace.Back, x0, y, z0);
                            }
                        }
                    }
                }
            }

            colliderMesh.vertices = vertices;
            colliderMesh.triangles = triangles;
            colliderMesh.uv = uvs;
        }

        [BurstCompile]
        private void FillBlockData(Vector2Int _id)
        {
            for (int y = 0; y < Height; y++)
            {
                yVolume = y * PlaneArea;

                for (int z = 1; z < Width + 1; z++)
                {
                    yCount = z * ExLength + yVolume;

                    for (int x = 1; x < Length + 1; x++)
                    {
                        blockArray[x + yCount] = BlockType.GrassBlock;
                    }
                }
            }
        }

        private int vIndex;
        private int tIndex;
        private int uvIndex;

        private void SetAllMesh(BlockProperty _block, int _x, int _y, int _z)
        {
            currentVertices = _block.mesh.GetAllVertices(_x, _y, _z);
            currentUVs = _block.mesh.GetAllUVs(_block);

            for (i = 0; i < currentVertices.Length; i++)
            {
                vertices[vIndex + i] = currentVertices[i];
            }

            for (i = 0; i < 24; i++)
            {
                uvs[uvIndex + i] = currentUVs[i];
            }

            vIndex += currentVertices.Length;
            uvIndex += 24;

            currentTriangles = _block.mesh.GetTriangles(vIndex);
            for (i = 0; i < currentTriangles.Length; i++)
            {
                triangles[tIndex + i] = currentTriangles[i];
            }

            tIndex += currentTriangles.Length;
        }

        private void SetMesh(BlockProperty _block, BlockFace _face, int _x, int _y, int _z)
        {
            currentVertices = _block.mesh.GetVertices(_face, _x, _y, _z);
            currentUVs = _block.mesh.GetUVs(_block, _face);

            for (i = 0; i < currentVertices.Length; i++)
            {
                vertices[vIndex + i] = currentVertices[i];
            }

            uvs[uvIndex] = currentUVs[0];
            uvs[uvIndex + 1] = currentUVs[1];
            uvs[uvIndex + 2] = currentUVs[2];
            uvs[uvIndex + 3] = currentUVs[3];

            vIndex += currentVertices.Length;
            uvIndex += 4;

            currentTriangles = _block.mesh.GetTriangles(vIndex);
            for (i = 0; i < currentTriangles.Length; i++)
            {
                triangles[tIndex + i] = currentTriangles[i];
            }

            tIndex += currentTriangles.Length;
        }

        private void EnsureCapacity(BlockProperty _block)
        {
            if (vIndex + 24 <= vertices.Length) return;

            Vector3[] v = new Vector3[vertices.Length + 8192];
            Array.ConstrainedCopy(vertices, 0, v, 0, vertices.Length);

            int[] t = new int[triangles.Length + 12288];
            Array.ConstrainedCopy(vertices, 0, t, 0, triangles.Length);
            triangles = t;

            Vector2[] uv = new Vector2[uvs.Length + 8192];
            Array.ConstrainedCopy(uvs, 0, uv, 0, uvs.Length);
            uvs = uv;
        }
    }
}