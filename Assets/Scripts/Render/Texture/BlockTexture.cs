namespace Render.Texture
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using Mesh;
    using Block;
    using Block.Blocks;
    using Utilities;

    public static class BlockTexture
    {
        public static Texture2D MainTexture;
        private const int WIDTH = 16;

        private static readonly Dictionary<string, UVPos> UVPosDict = new();

        private static bool mergeDone;
        private static UVPos uvPos;

        public static void Gen()
        {
            List<Texture2D> textureList = LoadTextures();

            int slices = GetSlices(textureList.Count);

            CreateTexture(slices);
            MergeTexture(textureList);

            // if MainTexture is too small, extend the size and re-merge
            if (!mergeDone)
            {
                CreateTexture(slices * 2);
                MergeTexture(textureList);
            }

            GameAssets.SavaTexture(MainTexture, GameAssets.BlockTexturePath, GameAssets.MainBlockTextureName);
        }

        private static void CreateTexture(int _slices)
        {
            int length = _slices * WIDTH;
            // set the main texture
            MainTexture = new Texture2D(length, length)
            {
                // set texture setting
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            MainTexture.SetPixels(Enumerable.Repeat(Color.clear, length * length).ToArray());
        }

        private static List<Texture2D> LoadTextures()
        {
            List<Texture2D> tList = new();
            List<Texture2D> texture2DList = new();

            foreach (BlockType block in Enum.GetValues(typeof(BlockType)))
            {
                texture2DList = GameAssets.GetAllBlockTexture(block);
                if (texture2DList != null)
                {
                    tList.AddRange(texture2DList);
                }
            }

            return tList;
        }

        private static void MergeTexture(List<Texture2D> _texture)
        {
            Vector2Int currentPos = Vector2Int.zero;

            foreach (Texture2D texture in _texture)
            {
                if (texture.width <= MainTexture.width - currentPos.x)
                {
                    if (texture.height <= MainTexture.height - currentPos.y)
                    {
                        AddToUVPosDict(currentPos, texture);
                        SetColors(currentPos, texture);
                        currentPos.y += texture.height;
                    }
                    else
                    {
                        currentPos.x += texture.width;
                        currentPos.y = 0;
                        AddToUVPosDict(currentPos, texture);
                        SetColors(currentPos, texture);
                        currentPos.y += texture.height;
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
        private static int GetSlices(int _i)
        {
            int i = 0;
            while (_i > 1)
            {
                _i /= 2;
                i++;
            }

            return i * 2;
        }

        private static void SetColors(Vector2Int _pos, Texture2D _new)
        {
            MainTexture.SetPixels(_pos.x, _pos.y, _new.width, _new.height, _new.GetPixels());
        }

        private static void AddToUVPosDict(Vector2Int _pos, Texture2D _texture)
        {
            // convert pixel position to uv position
            uvPos = new UVPos
            {
                startPoint = (Vector2)_pos / MainTexture.width,
                endPoint = new Vector2(
                    ((float)_pos.x + _texture.width) / MainTexture.width,
                    ((float)_pos.y + _texture.height) / MainTexture.height)
            };
            UVPosDict.TryAdd(_texture.name, uvPos);
        }

        /// <summary>
        /// return block face uv pos from Dictionary: UVPosDict
        /// </summary>
        /// <param name="_block">type of Block</param>
        /// <param name="_face">block face</param>
        /// <returns></returns>
        public static UVPos GetUVPos(Block _block, BlockFace _face)
        {
            return UVPosDict.TryGetValue(_block.TextureName(_face), out uvPos) ? uvPos : UVPosDict[_block.TextureName()];
        }
    }
}