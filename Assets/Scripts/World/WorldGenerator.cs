namespace World
{
    using UnityEngine;
    using Chunks;
    using Utilities;

    public class WorldGenerator : MonoBehaviour
    {
        private static readonly WorldData_SO WorldData = GameAssets.WorldData;
        private static readonly GameObject ChunkPrefab = GameAssets.ChunkPrefab;

        public static void GenChunks(Transform _parent)
        {
            Vector2Int id = new(0, 0);

            GameObject chunkInstance = Instantiate(ChunkPrefab, new Vector3(id.x * Chunk.Lenght, 0, id.y * Chunk.Width), Quaternion.identity, _parent);
            chunkInstance.name = $"Chunk{id}";
            Chunk chunk = chunkInstance.GetComponent<Chunk>();
            chunk.Gen(id);
        }
    }
}