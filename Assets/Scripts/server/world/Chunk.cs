using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    public Tweaks tweaks;
    [HideInInspector]public int numX, numZ;
    public GameObject solidChunkPrefab;

    private int _dx, _dz;

    public void InitMesh() {
        Mesh mesh = new Mesh();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();
        List<Vector3> vertices = new List<Vector3>();
        int y = 5;
        _dx = (numX + tweaks.viewDistance) * tweaks.chunkLength;
        _dz = (numZ + tweaks.viewDistance) * tweaks.chunkLength;
        
        int totalLength = 2 * tweaks.chunkLength * tweaks.viewDistance + tweaks.chunkLength;
        for (int x = _dx; x < _dx + tweaks.chunkLength; x++) {
            for (int z = _dz; z < _dz + tweaks.chunkLength; z++) {
                if (Block.IsBlock(x, y, z, BlockType.Air)) continue;
                int faces = 0;
                //top
                if (y == tweaks.chunkHeight - 1 || !BlockFromMesh(x, y + 1, z).OpaqueDown) {
                    vertices.AddRange(GetVertices(x, y, z, Direction.Top));
                    uvs.AddRange(GetUVs(x, y, z, TileType.CubeTop));
                    faces++;
                }

                //down
                if (y == 0 || !BlockFromMesh(x, y - 1, z).OpaqueTop) {
                    vertices.AddRange(GetVertices(x, y, z, Direction.Down));
                    uvs.AddRange(GetUVs(x, y, z, TileType.CubeDown));
                    faces++;
                }

                //front
                if (z == totalLength - 1 || !BlockFromMesh(x, y, z + 1).OpaqueSide) {
                    vertices.AddRange(GetVertices(x, y, z, Direction.Front));
                    uvs.AddRange(GetUVs(x, y, z));
                    faces++;
                }

                //back
                if (z == 0 || !BlockFromMesh(x, y, z - 1).OpaqueSide) {
                    vertices.AddRange(GetVertices(x, y, z, Direction.Back));
                    uvs.AddRange(GetUVs(x, y, z));
                    faces++;
                }

                //right
                if (x == totalLength - 1 || !BlockFromMesh(x + 1, y, z).OpaqueSide) {
                    vertices.AddRange(GetVertices(x, y, z, Direction.Right));
                    uvs.AddRange(GetUVs(x, y, z));
                    faces++;
                }

                //left
                if (x == 0 || !BlockFromMesh(x - 1, y, z).OpaqueSide) {
                    vertices.AddRange(GetVertices(x, y, z, Direction.Left));
                    uvs.AddRange(GetUVs(x, y, z));
                    faces++;
                }

                triangles.AddRange(GetTriangles(faces, vertices.Count - 4 * faces));

            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

       solidChunkPrefab.GetComponent<MeshFilter>().mesh = mesh;
       solidChunkPrefab.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void RefreshMesh() {

        Mesh chunkMesh = new Mesh();
        List<Vector2> chunkUVs = new List<Vector2>();
        List<int> chunkTriangles = new List<int>();
        List<Vector3> chunkVertices = new List<Vector3>();
        
        Mesh solidChunkMesh = new Mesh();
        List<Vector2> solidChunkUVs = new List<Vector2>();
        List<int> solidChunkTriangles = new List<int>();
        List<Vector3> solidChunkVertices = new List<Vector3>();

        _dx = (numX + tweaks.viewDistance) * tweaks.chunkLength;
        _dz = (numZ + tweaks.viewDistance) * tweaks.chunkLength;
        
        int totalLength = 2 * tweaks.chunkLength * tweaks.viewDistance + tweaks.chunkLength;
        for (int x = _dx; x < _dx + tweaks.chunkLength; x++) {
            for (int y = 0; y < tweaks.chunkHeight; y++) {
                for (int z = _dz; z < _dz + tweaks.chunkLength; z++) {
                    if (!Block.IsBlock(x, y, z, BlockType.Air)) {
                        // grass / flower
                        if (BlockFromMesh(x, y, z).CrossTileType) {
                            chunkVertices.AddRange(GetCrossVertices(x, y, z));
                            chunkUVs.AddRange(GetUVs(x, y, z));
                            chunkUVs.AddRange(GetUVs(x, y, z));
                            chunkUVs.AddRange(GetUVs(x, y, z));
                            chunkUVs.AddRange(GetUVs(x, y, z));
                            chunkTriangles.AddRange(GetTriangles(4, chunkVertices.Count - 4 * 4));
                            continue;
                        }
                        
                        int faces = 0;
                        //top
                        if (y == tweaks.chunkHeight - 1 || !BlockFromMesh(x, y + 1, z).OpaqueDown) {
                            solidChunkVertices.AddRange(GetVertices(x, y, z, Direction.Top));
                            solidChunkUVs.AddRange(GetUVs(x, y, z, TileType.CubeTop));
                            faces++;
                        }

                        //down
                        if (y == 0 || !BlockFromMesh(x, y - 1, z).OpaqueTop) {
                            solidChunkVertices.AddRange(GetVertices(x, y, z, Direction.Down));
                            solidChunkUVs.AddRange(GetUVs(x, y, z, TileType.CubeDown));
                            faces++;
                        }

                        //front
                        if (z == totalLength - 1 || !BlockFromMesh(x, y, z + 1).OpaqueSide) {
                            solidChunkVertices.AddRange(GetVertices(x, y, z, Direction.Front));
                            solidChunkUVs.AddRange(GetUVs(x, y, z));
                            faces++;
                        }

                        //back
                        if (z == 0 || !BlockFromMesh(x, y, z - 1).OpaqueSide) {
                            solidChunkVertices.AddRange(GetVertices(x, y, z, Direction.Back));
                            solidChunkUVs.AddRange(GetUVs(x, y, z));
                            faces++;
                        }

                        //right
                        if (x == totalLength - 1 || !BlockFromMesh(x + 1, y, z).OpaqueSide) {
                            solidChunkVertices.AddRange(GetVertices(x, y, z, Direction.Right));
                            solidChunkUVs.AddRange(GetUVs(x, y, z));
                            faces++;
                        }

                        //left
                        if (x == 0 || !BlockFromMesh(x - 1, y, z).OpaqueSide) {
                            solidChunkVertices.AddRange(GetVertices(x, y, z, Direction.Left));
                            solidChunkUVs.AddRange(GetUVs(x, y, z));
                            faces++;
                        }

                        solidChunkTriangles.AddRange(GetTriangles(faces, solidChunkVertices.Count - 4 * faces));
                    }
                }
            }
        }
        chunkMesh.vertices = chunkVertices.ToArray();
        chunkMesh.triangles = chunkTriangles.ToArray();
        chunkMesh.uv = chunkUVs.ToArray();
        chunkMesh.RecalculateNormals();
        
        GetComponent<MeshFilter>().mesh = chunkMesh;
        //GetComponent<MeshCollider>().sharedMesh = chunkMesh;
        
        solidChunkMesh.vertices = solidChunkVertices.ToArray();
        solidChunkMesh.triangles = solidChunkTriangles.ToArray();
        solidChunkMesh.uv = solidChunkUVs.ToArray();
        solidChunkMesh.RecalculateNormals();
        
        solidChunkPrefab.GetComponent<MeshFilter>().mesh = solidChunkMesh;
        solidChunkPrefab.GetComponent<MeshCollider>().sharedMesh = solidChunkMesh;
    }    
    private BlockMesh BlockFromMesh(int x, int y, int z) {
        return BlockMesh.BlockTilePos[Block.GetBlock(x, y, z)];
    }
    
    private Vector3[] GetVertices(int x, int y, int z, Direction direction) {
        return BlockMesh.CubeVertices(new Vector3(x - _dx, y, z - _dz), direction);
    }
    
    private Vector3[] GetCrossVertices(int x, int y, int z) {
        return BlockMesh.CrossVertices(new Vector3(x - _dx + Random.Range(-0.3f, 0.3f), y, z - _dz + Random.Range(-0.3f, 0.3f)));
    }
    
    private Vector2[] GetUVs(int x, int y, int z, TileType tileType = TileType.CubeSide) {
        return BlockFromMesh(x, y, z).UVs(tileType);
    }

    private int[] GetTriangles(int i, int nums) {
        return BlockMesh.Triangles(i, nums);
    }
}
