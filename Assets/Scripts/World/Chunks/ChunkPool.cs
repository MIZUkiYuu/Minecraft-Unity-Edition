using Utilities;

namespace World.Chunks
{
    public class ChunkPool : BaseGameObjectPool<Chunk>
    {
        private void Awake()
        {
            int length = worldDataSO.viewDistance * 2 + 1;
            int chunkCount = length * length;
            Initialize(chunkCount, chunkCount);
        }
    }
}