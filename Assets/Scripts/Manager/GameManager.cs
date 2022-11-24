using Render.Texture;
using UnityEngine;
using World;
using World.Chunks;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public WorldData_SO WorldData;

        [Header("Chunk")] public ChunkPool chunkPool;
        
        private void Start()
        {
            BlockTexture.Instance.Gen();
            WorldGenerator.Instance.GenChunks(WorldData, chunkPool);
        }
    }
}