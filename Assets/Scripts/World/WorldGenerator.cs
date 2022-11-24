namespace World
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    using UnityEngine;
    using Chunks;

    public class WorldGenerator : MonoBehaviour
    {
        public static readonly WorldGenerator Instance = new();

        public void GenChunks(WorldData_SO _worldDataSo, ChunkPool _chunkPool)
        {
            Vector2Int id = Vector2Int.zero;

            int index;
            int zCount;
            int viewDistance = _worldDataSo.viewDistance;
            int length = 2 * viewDistance + 1;
            int chunkNum = length * length;

            ChunkGenerator chunkGenerator = new();
            Mesh[] meshes = new Mesh[chunkNum];
            Chunk[] chunks = new Chunk[chunkNum];

            for (int z = -viewDistance; z <= viewDistance; z++)
            {
                zCount = length * (z + viewDistance) + viewDistance;
                for (int x = -viewDistance; x <= viewDistance; x++)
                {
                    id.Set(x, z);
                    index = x + zCount;

                    chunks[index] = _chunkPool.Get();
                    chunks[index].name = $"Chunk{id}";
                    chunks[index].transform.position = new Vector3(id.x * Chunk.Length, 0, id.y * Chunk.Width);

                    chunkGenerator.Gen(chunks[index], id);
                    meshes[index] = chunkGenerator.colliderMesh;
                }
            }

            NativeArray<int> meshIds = new(meshes.Length, Allocator.TempJob);

            for (int i = 0; i < meshes.Length; ++i)
            {
                meshIds[i] = meshes[i].GetInstanceID();
            }

            BakeJob job = new(meshIds);
            job.Schedule(meshes.Length, 10).Complete();

            meshIds.Dispose();

            for (int i = 0; i < meshes.Length; ++i)
            {
                chunks[i].SetComponent(meshes[i]);
            }
        }
    }

    [BurstCompile]
    public readonly struct BakeJob : IJobParallelFor
    {
        private readonly NativeArray<int> meshIds;

        public BakeJob(NativeArray<int> _meshIds)
        {
            meshIds = _meshIds;
        }

        public void Execute(int _index)
        {
            Physics.BakeMesh(meshIds[_index], false);
        }
    }
}