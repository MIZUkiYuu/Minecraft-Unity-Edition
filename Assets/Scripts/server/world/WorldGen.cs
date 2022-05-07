using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldGen : MonoBehaviour {
    public GameObject player;
    public GameObject chunkPrefab;
    public static List<int> SurfaceYPos = new();
    public Tweaks tweaks;
    
    [SerializeField]public List<Chunk> chunks = new();
    private float _randomX, _randomZ;   // random parameter of Perlin Noise
    private float _relief;
    private Vector3 _playerPos;
    private bool _regenChunks;

    private void Start() {
        Random.InitState(tweaks.seed);
        _randomX = Random.value * 100;
        _randomZ = Random.value * 100;

        WorldBlocksGen();
        PlayerSpawn();
        GenChunks();
    }

    private void Update() {
    }

    private int ChunksTotalLength() {
        return (2 * tweaks.viewDistance + 1) * tweaks.chunkLength;
    }
    
    
    private void WorldBlocksGen() {   // generate the terrain                                                         
        for (int x = 0; x < ChunksTotalLength(); x++) {
            for (int z = 0; z < ChunksTotalLength(); z++) {
                
                int y = GetYFromPerlinNoise(x, z) < 5 ? 5 : GetYFromPerlinNoise(x, z);
                
                Block.SetBlock(x, y, z, BlockType.GrassBlock);  // the surface
                
                for (int i = 0; i < tweaks.chunkHeight; i++)
                {
                    if (i <= 4) Block.SetBlock(x, i, z, BlockType.Bedrock);
                    if (4 < i && i < y) Block.SetBlock(x, i, z, BlockType.Dirt);
                }
                Plant.Generation(x, y + 1, z);
            }
        }
    }

    //use Perlin noise to generate the value of y
    private int GetYFromPerlinNoise(int x, int z) {
        float xSample = (x * tweaks.xScale + _randomX) / tweaks.relief;
        float zSample = (z * tweaks.zScale + _randomZ) / tweaks.relief;
        float yNoise = Math.Clamp(Mathf.PerlinNoise(xSample, zSample), 0, 1);
        yNoise = Astroid(yNoise);
        return (int)(yNoise * tweaks.terrainHeightMax);
    }

    private void GenChunks() {
        // generate chunks
        // total chunks = (2 * viewDistance + 1)^ 2
        for (int numX = - tweaks.viewDistance; numX <= tweaks.viewDistance; numX++) {
            for (int numZ = - tweaks.viewDistance; numZ <= tweaks.viewDistance; numZ++) {
                GameObject chunkInstance = Instantiate(chunkPrefab, new Vector3(ChunkPosPlayerIn().x + numX * tweaks.chunkLength, 0, ChunkPosPlayerIn().y + 
                numZ * tweaks.chunkLength), Quaternion.identity, transform);
                chunkInstance.name = $"Chunk({numX}, {numZ})"; // rename chunk -> Chunk(0, 0)
                Chunk chunk = chunkInstance.GetComponent<Chunk>();
                chunks.Add(chunk); // store chunk to chunk list
                chunk.numX = numX;
                chunk.numZ = numZ;
                chunk.RefreshMesh();
            }
        }
    }

    private static float Astroid(float x) {
        return Mathf.Pow(1 - Mathf.Pow(1 - x, 1.0f / 2), 2);    //y^(1/2) + x^(1/2) = 1 
    }
    
    private void PlayerSpawn() {
        int x = tweaks.playerSpawnPoint.x;
        int z = tweaks.playerSpawnPoint.y;
        _playerPos = player.transform.position = tweaks.playerSpawnRadius == 0
            ? new Vector3(x - 0.5f, Block.GetTopBlockHeight(x, z) + 5, z - 0.5f)
            : new Vector3Int((int)(Random.insideUnitCircle.x * tweaks.playerSpawnRadius) + x,
                Block.GetTopBlockHeight(x, z) + 5, (int)(Random.insideUnitCircle.y * tweaks.playerSpawnRadius) + z);
    }

    private Vector2Int ChunkPosPlayerIn() { // the lower-left point of chunk which player is in
        return new Vector2Int((Mathf.FloorToInt(_playerPos.x) >> 4) * tweaks.chunkLength,
            (Mathf.FloorToInt(_playerPos.z) >> 4) * tweaks.chunkLength);
    }
}
