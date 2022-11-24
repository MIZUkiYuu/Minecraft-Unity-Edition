namespace World.Chunks
{
    using UnityEngine;
    using Block;

    public class Chunk : MonoBehaviour
    {
        public const int Length = 16;
        public const int Width = 16;
        public const int Height = 256;
        public static readonly Vector3Int Size = new(Length, Height, Width);

        [HideInInspector] public Vector2Int id; // chunk number <- Chunk(x, z)
        [HideInInspector] public BlockType[] blockArray = new BlockType[(Length + 2) * Height * (Width + 2)];

        public int TopBlock { get; set; }
        public int BottomBlock { get; set; }
        public static Vector2Int Position { get; set; } // the south-west corner where the y-axis is 0.
        public Vector3Int TopBlockPos { get; set; }
        public Vector3Int BottomBlockPos { get; set; }

        [HideInInspector] public Mesh colliderMesh;

        [HideInInspector] public MeshFilter meshFilter;

        [HideInInspector] public MeshCollider meshCollider;
        // [HideInInspector] public MeshRenderer meshRenderer;


        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            // meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();
        }

        public void SetComponent(Mesh _colliderMesh)
        {
            _colliderMesh.RecalculateNormals();
            _colliderMesh.RecalculateBounds();
            meshFilter.mesh = _colliderMesh;
            meshCollider.sharedMesh = _colliderMesh;
        }
    }
}