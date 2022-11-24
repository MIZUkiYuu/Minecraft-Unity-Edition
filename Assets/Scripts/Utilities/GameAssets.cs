using Block;
using Render.Texture;
using World;

namespace Utilities
{

    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public static class GameAssets
    {
        public const string MainBlockTextureName = "MainBlockTexture.png";

        // path
        public const string GameAssetsPath = "Assets/GameAssets/";
        public const string BlockTexturePath = GameAssetsPath + "Textures/Block/";
        public const string MaterialPath = GameAssetsPath + "Materials/Block/";
        
        public static Material MainBlockMaterial = GetMaterial("MainBlockMat.mat");
        
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