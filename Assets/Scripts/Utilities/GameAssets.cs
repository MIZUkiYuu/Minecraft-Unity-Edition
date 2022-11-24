using Render.Texture;
using World;

namespace Utilities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using UnityEditor;
    using UnityEngine;
    using Block.Blocks;

    public static class GameAssets
    {
        public const string MainBlockTextureName = "MainBlockTexture.png";

        // path
        public const string GameAssetsPath = "Assets/GameAssets/";
        public const string BlockTexturePath = GameAssetsPath + "Textures/Block/";
        public const string MaterialPath = GameAssetsPath + "Materials/Block/";
        
        public static readonly Material MainBlockMaterial = GetMaterial("MainBlockMat.mat");

        public static readonly WorldData_SO WorldData = AssetDatabase.LoadAssetAtPath<WorldData_SO>("Assets/GameData/WorldData.asset");
        public static readonly GameObject ChunkPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/CHUNK_prefab.prefab");

        // return all block textures list
        public static List<Texture2D> GetAllBlockTexture(BlockType _block)
        {
            FileInfo[] files = new DirectoryInfo(GetBlockTextureDir(_block)).GetFiles("*.png");

            return files.Length == 0
                ? null
                : files.Select(_file => AssetDatabase.LoadAssetAtPath<Texture2D>(GetBlockTextureDir(_block) + _file.Name)).ToList();
        }

        /// <summary>
        /// return the path of block type directory
        /// </summary>
        /// <param name="_block"></param>
        /// <returns></returns>
        public static string GetBlockTextureDir(BlockType _block)
        {
            return BlockTexturePath + $"/{_block.ToString()}/";
        }

        /// <summary>
        /// return the material by name from file
        /// </summary>
        /// <param name="_name">material name</param>
        /// <returns></returns>
        public static Material GetMaterial(string _name)
        {
            return AssetDatabase.LoadAssetAtPath<Material>(MaterialPath + _name);
        }

        /// <summary>
        /// save the texture
        /// </summary>
        /// <param name="_texture">target texture</param>
        /// <param name="_path">path</param>
        /// <param name="_name">file name ending in ".png"</param>
        public static void SavaTexture(Texture2D _texture, string _path, string _name)
        {
            byte[] dataBytes = _texture.EncodeToPNG();
            FileStream fileStream = File.Open(_path + _name, FileMode.OpenOrCreate);
            fileStream.Write(dataBytes, 0, dataBytes.Length);
            fileStream.Close();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}