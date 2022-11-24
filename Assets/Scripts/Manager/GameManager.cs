using Render.Texture;
using UnityEngine;
using World;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("chunk")] public Transform chunksParent;
        
        private void Start()
        {
            BlockTexture.Gen();
            WorldGenerator.GenChunks(chunksParent);
        }
    }
}