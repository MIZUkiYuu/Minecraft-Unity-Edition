using System.IO;
using System.Linq;
using Block.Blocks;
using Render.Mesh;
using UnityEditor;
using UnityEngine;
using Utilities;
using FileInfo = Codice.Client.BaseCommands.Fileinfo.FileInfo;

namespace Editor
{
    public static class Test
    {
        [MenuItem("Test/Log01", false, 1)]
        private static void Log01()
        {
        }

        [MenuItem("Test/Log02", false, 1)]
        private static void Log02()
        {
            System.IO.FileInfo[] files = new DirectoryInfo(GameAssets.GetBlockTextureDir(BlockType.GrassBlock)).GetFiles("*.png");

            foreach (var v in files)
            {
                Debug.Log(v.Name);
            }
        }
        
    }
}