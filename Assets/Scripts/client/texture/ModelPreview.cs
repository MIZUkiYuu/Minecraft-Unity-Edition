using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelPreview : MonoBehaviour {
    public Inventory inventory;
    public Camera cameraPre;
    public GameObject block;
    public Transform toolbarSlot;
    
    private const int Width = 256;
    private const int Height = 256;
    private RenderTexture _renderTexture;
    private Texture2D _texture2D;
    
    public static readonly Dictionary<BlockType, Texture> BlockTexture2Ds = new();

    private void Start() {
        _renderTexture = new RenderTexture(Width, Height, 16);

        foreach (BlockType blockType in Enum.GetValues(typeof(BlockType))) {
            BuildBlockMesh(blockType);
            _texture2D = new Texture2D(Width, Height) {
                name = blockType.ToString()
            };
            cameraPre.targetTexture = _renderTexture;
            RenderTexture.active = _renderTexture;
            cameraPre.Render();
            _texture2D.ReadPixels(new Rect(0, 0, Width, Height),0, 0);
            _texture2D.Apply();
            BlockTexture2Ds.Add(blockType, _texture2D);
        }
        
        //set texture to every item in toolbar
        for (int i = 0; i < inventory.toolbar.Length; i++) {
            toolbarSlot.GetChild(i).GetChild(0).GetChild(0).GetComponent<RawImage>().texture = ModelPreview.BlockTexture2Ds[inventory.GetBlockType
                (InventoryType.Toolbar, i)];
        }
    }

    private void BuildBlockMesh(BlockType blockType) {
        Mesh mesh = new ();
        List<Vector2> blockUVs = new ();
        List<int> blockTriangles = new ();
        List<Vector3> blockVertices = new ();

        BlockMesh blockMesh = BlockMesh.BlockTilePos[blockType];
        Vector2[] blockMeshUVs = blockMesh.UVs();
        
        // grass , flowers
        if (blockMesh.CrossTileType) {
            blockVertices.AddRange(BlockMesh.CrossVertices(new Vector3(0 , 0, 0 )));
            blockUVs.AddRange(blockMeshUVs);
            blockUVs.AddRange(blockMeshUVs);
            blockUVs.AddRange(blockMeshUVs);
            blockUVs.AddRange(blockMeshUVs);
            blockTriangles.AddRange(BlockMesh.Triangles(4, 0));
        }
        else {
            blockVertices.AddRange(BlockMesh.CubeVertices(new Vector3(0, 0, 0), Direction.Top));
            blockUVs.AddRange(blockMesh.UVs(TileType.CubeTop));

            blockVertices.AddRange(BlockMesh.CubeVertices(new Vector3(0, 0, 0), Direction.Down));
            blockUVs.AddRange(blockMesh.UVs(TileType.CubeDown));

            blockVertices.AddRange(BlockMesh.CubeVertices(new Vector3(0, 0, 0), Direction.Front));
            blockUVs.AddRange(blockMeshUVs);

            blockVertices.AddRange(BlockMesh.CubeVertices(new Vector3(0, 0, 0), Direction.Back));
            blockUVs.AddRange(blockMeshUVs);

            blockVertices.AddRange(BlockMesh.CubeVertices(new Vector3(0, 0, 0), Direction.Right));
            blockUVs.AddRange(blockMeshUVs);

            blockVertices.AddRange(BlockMesh.CubeVertices(new Vector3(0, 0, 0), Direction.Left));
            blockUVs.AddRange(blockMeshUVs);

            blockTriangles.AddRange(BlockMesh.Triangles(6, 0));
        }

        mesh.vertices = blockVertices.ToArray();
        mesh.triangles = blockTriangles.ToArray();
        mesh.uv = blockUVs.ToArray(); 
        mesh.RecalculateNormals();

        block.GetComponent<MeshFilter>().mesh = mesh;
    }
}
