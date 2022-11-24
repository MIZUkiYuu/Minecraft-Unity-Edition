namespace Render.Texture
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using UnityEditor;
    using UnityEngine;
    using Mesh;
    using Block;
    using Utilities;

    public class BlockTexture
    {
        public static Texture2D MainTexture;
        public static readonly BlockTexture Instance = new();

        private const int DefaultWidth = 16;
        private static readonly int BlockTypeLength = Enum.GetValues(typeof(BlockType)).Length;

        public static readonly UVPos[] UVPosArray = new UVPos[1000];

        public static readonly int[,] BlockTextureIndex = new int[BlockTypeLength - 1, 6]; // ignore Air

        private bool mergeDone;

        public void Gen()
        {
            Texture2D[] textureList = LoadTextures();

            int slices = GetSlices(textureList.Length);

            CreateTexture(slices);
            MergeTexture(textureList);

            // if MainTexture is too small, extend the size and re-merge
            if (!mergeDone)
            {
                CreateTexture(slices * 2);
                MergeTexture(textureList);
            }

            GameAssets.MainBlockMaterial.mainTexture = MainTexture;

            // GameAssets.SavaTexture(MainTexture, GameAssets.BlockTexturePath, GameAssets.MainBlockTextureName);
        }

        private void CreateTexture(int _slices)
        {
            int length = _slices * DefaultWidth;
            // set the main texture
            MainTexture = new Texture2D(length, length)
            {
                // set texture setting
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            MainTexture.SetPixels(Enumerable.Repeat(Color.clear, length * length).ToArray());
        }

        private Texture2D[] LoadTextures()
        {
            List<Texture2D> texture2DList = new();
            List<Texture2D> currentList = new();
            string path;
            string name;
            int count;
            int index;

            for (int i = 1; i < BlockTypeLength; i++)
            {
                path = GameAssets.GetBlockTextureDir((BlockType)i);
                FileInfo[] files = new DirectoryInfo(path).GetFiles("*.png");
                currentList = files.Length == 0 ? null : files.Select(_file => AssetDatabase.LoadAssetAtPath<Texture2D>(path + _file.Name)).ToList();

                if (currentList == null) continue;

                name = BlockManager.BlockDict[i].name; // block name
                count = texture2DList.Count; // the number of texture of this block type

                // 将获取到的图片贴图进行编码，二维数组第一位为方块种类（Enum： BlockType）的序号，第二位为贴图方位（Enum：TextureSuffix）的序号
                index = 1; // index 0 is default texture
                for (int j = 0; j < 6; j++)
                {
                    if (name + TextureSuffix.Face[j] == currentList[index].name)
                    {
                        BlockTextureIndex[i - 1, j] = count + index;
                        index++;
                    }
                    else
                    {
                        BlockTextureIndex[i - 1, j] = count;
                    }
                }

                texture2DList.AddRange(currentList);
            }

            return texture2DList.ToArray();
        }

        private void MergeTexture(Texture2D[] _texture2Ds)
        {
            Vector2Int currentPos = Vector2Int.zero;
            int mainWidth = MainTexture.width;
            int mainHeight = MainTexture.height;
            Texture2D texture;
            int width;
            int height;

            for (int i = 0; i < _texture2Ds.Length; i++)
            {
                texture = _texture2Ds[i];
                width = texture.width;
                height = texture.height;
                if (width <= mainWidth - currentPos.x)
                {
                    if (height <= mainHeight - currentPos.y)
                    {
                        AddToUVPosDict(currentPos, texture, i, mainWidth, mainHeight);
                        SetColors(currentPos, texture);
                        currentPos.y += texture.height;
                    }
                    else
                    {
                        currentPos.x += width;
                        currentPos.y = 0;
                        AddToUVPosDict(currentPos, texture, i, mainWidth, mainHeight);
                        SetColors(currentPos, texture);
                        currentPos.y += height;
                    }
                }
                else
                {
                    mergeDone = false;
                    return;
                }
            }

            mergeDone = true;
            MainTexture.Apply();
        }

        // return the sqr root in Integer
        private int GetSlices(int _i)
        {
            int i = 0;
            while (_i > 1)
            {
                _i /= 2;
                i++;
            }

            return i * 2;
        }

        private void SetColors(Vector2Int _pos, Texture2D _new)
        {
            MainTexture.SetPixels(_pos.x, _pos.y, _new.width, _new.height, _new.GetPixels());
        }

        private float x, y;
        private UVPos uvPos;

        private void AddToUVPosDict(Vector2Int _pos, Texture2D _texture, int _index, int _w, int _h)
        {
            x = _pos.x;
            y = _pos.y;
            // convert pixel position to uv position
            uvPos.x0 = x / _w;
            uvPos.y0 = y / _h;
            uvPos.x1 = (x + _texture.width) / _w;
            uvPos.y1 = (y + _texture.height) / _h;
            UVPosArray[_index] = uvPos;
        }
    }
}