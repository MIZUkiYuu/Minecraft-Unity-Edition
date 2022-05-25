using UnityEngine;

[CreateAssetMenu(fileName = "Tweaks", menuName = "ScriptableObjects/Tweaks")]
public class Tweaks : ScriptableObject {
    
    [Header("---- Spawn ----")] 
    public Vector2Int playerSpawnPoint = new Vector2Int(0, 0);
    public int playerSpawnRadius = 10;
    
    [Header("---- Movement ----")]
    public float moveSpeed;
    public float maxJumpHeight;

    [Header("---- ViewRotate ----")]
    public float sensitivity;
    public float horizontalSensitivity;
    public float verticalSensitivity;

    [Header("---- Operate ----")]
    public float maxOperateDistance;

    [Header("---- World ----")]
    public int seed = 0;
    [Tooltip("Maximum number of unilateral chunks")]
    public int maxWorldSize = 210;
    public int viewDistance = 10;
    
    [Header("---- Chunk ----")]
    public int chunkLength = 16;
    public int chunkHeight = 64;

    [Header("---- Terrain Gen Parameter ----")]
    public float relief = 100;
    public int terrainHeightMax = 64;
    public float xScale = 1;
    public float zScale = 1;
}
