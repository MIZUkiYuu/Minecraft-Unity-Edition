using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class TextureImportSetting : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.isReadable = true;
            textureImporter.wrapMode = TextureWrapMode.Clamp;
            textureImporter.filterMode = FilterMode.Point;
            
            TextureImporterPlatformSettings textureImporterPlatformSettings = new()
            {
                format = TextureImporterFormat.RGBA32,
                textureCompression = TextureImporterCompression.Uncompressed,
                overridden = true
            };
            
            textureImporter.SetPlatformTextureSettings(textureImporterPlatformSettings);
            
        }
        
    }
}