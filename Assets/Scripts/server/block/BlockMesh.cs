using System;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    CubeSide, CubeTop, CubeDown
}

public class BlockMesh
{
    private int _side, _top, _down;
    private float _xPos, _yPos;
    private const float Tiles = 16.0f;   // 16 * 16 tiles in one block_texture.png
    private const float Width = 1 / 16.0f;   // the width of single tile
    private const float Crop = 0.001f;   // Avoid crop errors in Unity
    private const float CrossPointB = 0.85355339f;   // (2 + √2)/4
    private const float CrossPointS = 0.1464466f;    // (2 - √2)/4

    public bool OpaqueSide;
    public bool OpaqueTop;
    public bool OpaqueDown;
    public bool CrossTileType;
    private BlockMesh(int side, bool opaque = true, bool cross = false)
    {
        _side = _top = _down = side;    // The six faces are of the same texture
        OpaqueSide = OpaqueTop = OpaqueDown = opaque;
        CrossTileType = cross;
    }

    private BlockMesh(int side, int top, bool opaqueSide = true, bool opaqueTop = true, bool cross = false)
    {
        _side = side;
        _top = _down = top;
        OpaqueSide = opaqueSide;
        OpaqueTop = OpaqueDown = opaqueTop;
        CrossTileType = cross;
    }

    private BlockMesh(int side, int top, int down, bool opaqueSide = true, bool opaqueTop = true, bool opaqueDown = true)
    {
        _side = side;
        _top = top;
        _down = down;
        OpaqueSide = opaqueSide;
        OpaqueTop = opaqueTop;
        OpaqueDown = opaqueDown;
    }

    private Vector2[] CubeTilePos(int num)
    {
        _xPos = (int)(num / 16) / Tiles;
        _yPos = num % 16 / Tiles;
        return new Vector2[4]
        {
            new Vector2(_xPos + Width - Crop, _yPos + Width - Crop), new Vector2(_xPos + Crop, _yPos + Width - Crop),
            new Vector2(_xPos + Width - Crop, _yPos + Crop), new Vector2(_xPos + Crop, _yPos + Crop)
        };
    }
    
    public static Vector3[] CubeVertices(Vector3 blockPos, Direction direction)
    {
        return direction switch
        {
            Direction.Top => new[]
            {
                blockPos + new Vector3(1, 1, 1), blockPos + new Vector3(0, 1, 1), 
                blockPos + new Vector3(1, 1, 0), blockPos + new Vector3(0, 1, 0)
            },
            Direction.Down => new[]
            {
                blockPos + new Vector3(0, 0, 1), blockPos + new Vector3(1, 0, 1), 
                blockPos + new Vector3(0, 0, 0), blockPos + new Vector3(1, 0, 0)
            },
            Direction.Front => new[]
            {
                blockPos + new Vector3(0, 1, 1), blockPos + new Vector3(1, 1, 1), 
                blockPos + new Vector3(0, 0, 1), blockPos + new Vector3(1, 0, 1)
            },
            Direction.Back => new[]
            {
                blockPos + new Vector3(1, 1, 0), blockPos + new Vector3(0, 1, 0), 
                blockPos + new Vector3(1, 0, 0), blockPos + new Vector3(0, 0, 0)
            },
            Direction.Right => new[]
            {
                blockPos + new Vector3(1, 1, 1), blockPos + new Vector3(1, 1, 0), 
                blockPos + new Vector3(1, 0, 1), blockPos + new Vector3(1, 0, 0)
            },
            Direction.Left => new[]
            {
                blockPos + new Vector3(0, 1, 0), blockPos + new Vector3(0, 1, 1), 
                blockPos + new Vector3(0, 0, 0), blockPos + new Vector3(0, 0, 1)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static Vector3[] CrossVertices(Vector3 blockPos)
    {
        return new[]
        {
            // south-east
            blockPos + new Vector3(CrossPointB, 1-Crop, CrossPointB), blockPos + new Vector3(CrossPointS, 1-Crop, CrossPointS),
            blockPos + new Vector3(CrossPointB, 0-Crop, CrossPointB), blockPos + new Vector3(CrossPointS, 0-Crop, CrossPointS),
            // north-east
            blockPos + new Vector3(CrossPointS, 1-Crop, CrossPointB), blockPos + new Vector3(CrossPointB, 1-Crop, CrossPointS),
            blockPos + new Vector3(CrossPointS, 0-Crop, CrossPointB), blockPos + new Vector3(CrossPointB, 0-Crop, CrossPointS),
            // north-west
            blockPos + new Vector3(CrossPointS, 1-Crop, CrossPointS), blockPos + new Vector3(CrossPointB, 1-Crop, CrossPointB),
            blockPos + new Vector3(CrossPointS, 0-Crop, CrossPointS), blockPos + new Vector3(CrossPointB, 0-Crop, CrossPointB),
            //south-west
            blockPos + new Vector3(CrossPointB, 1-Crop, CrossPointS), blockPos + new Vector3(CrossPointS, 1-Crop, CrossPointB),
            blockPos + new Vector3(CrossPointB, 0-Crop, CrossPointS), blockPos + new Vector3(CrossPointS, 0-Crop, CrossPointB),
        };
    }

    public static int[] Triangles(int faces, int nums)
    {
        List<int> tri = new List<int>();
        for (int i = 0; i < faces; i++)
        {
            tri.AddRange(new[] {nums + i * 4, nums + i * 4 + 3, nums + i * 4 + 1, nums + i * 4, nums + i * 4 + 2, nums + i * 4 + 3});
        }
        return tri.ToArray();
    }

    public Vector2[] UVs(TileType to = TileType.CubeSide, bool twoSide = false)
    {
        switch (to)
        {
            case TileType.CubeTop:
                return CubeTilePos(_top);
            case TileType.CubeDown:
                return CubeTilePos(_down);
            default:
                return CubeTilePos(_side);
        }
    }
    

    public static readonly Dictionary<BlockType, BlockMesh> BlockTilePos = new()
    {
        {BlockType.Air, new BlockMesh(25, false)},
        {BlockType.GrassBlock, new BlockMesh(0,1,2)},
        {BlockType.Dirt, new BlockMesh(2)},
        {BlockType.Grass, new BlockMesh(3, false ,true)},
        {BlockType.TallGrass, new BlockMesh(4, 5,false, false, true)},
        {BlockType.LilyOfTheValley, new BlockMesh(6, false, true)},
        {BlockType.Dandelion, new BlockMesh(7, false, true)},
        {BlockType.Poppy, new BlockMesh(8, false, true)},
        {BlockType.Allium, new BlockMesh(9, false, true)},
        {BlockType.OxeyeDaisy, new BlockMesh(10, false, true)},
        {BlockType.WhiteTulip, new BlockMesh(11, false, true)},
        {BlockType.OrangeTulip, new BlockMesh(12, false, true)},
        {BlockType.PinkTulip, new BlockMesh(13, false, true)},
        {BlockType.Peony, new BlockMesh(14, 15, false, false, true)},
        {BlockType.Cobblestone, new BlockMesh(16)},
        {BlockType.Stone, new BlockMesh(17)},
        {BlockType.SmoothStone, new BlockMesh(18)},
        {BlockType.StoneBricks, new BlockMesh(19)},
        {BlockType.CrackedStoneBricks, new BlockMesh(20)},
        {BlockType.ChiseledStoneBricks, new BlockMesh(21)},
        {BlockType.Bedrock, new BlockMesh(22)},
        {BlockType.Obsidian, new BlockMesh(23)},
        {BlockType.Gravel, new BlockMesh(24)},
        {BlockType.Andesite, new BlockMesh(25)},
        {BlockType.PolishedAndesite, new BlockMesh(26)},
        {BlockType.Diorite, new BlockMesh(27)},
        {BlockType.PolishedDiorite, new BlockMesh(28)},
        {BlockType.Granite, new BlockMesh(29)},
        {BlockType.PolishedGranite, new BlockMesh(30)},
        {BlockType.Bricks, new BlockMesh(31)},
        {BlockType.CoalOre, new BlockMesh(32)},
        {BlockType.IronOre, new BlockMesh(33)},
        {BlockType.DiamondOre, new BlockMesh(34)},
        {BlockType.GoldOre, new BlockMesh(35)},
        {BlockType.RedstoneOre, new BlockMesh(36)},
        {BlockType.CoalBlock, new BlockMesh(37)},
        {BlockType.IronBlock, new BlockMesh(38)},
        {BlockType.DiamondBlock, new BlockMesh(39)},
        {BlockType.GoldBlock, new BlockMesh(40)},
        {BlockType.RedstoneBlock, new BlockMesh(41)},
        {BlockType.Sand, new BlockMesh(42)},
        {BlockType.AcaciaPlanks, new BlockMesh(43)},
        {BlockType.BirchPlanks, new BlockMesh(44)},
        {BlockType.DarkOakPlanks, new BlockMesh(45)},
        {BlockType.JunglePlanks, new BlockMesh(46)},
        {BlockType.OakPlanks, new BlockMesh(47)},
        {BlockType.SprucePlanks, new BlockMesh(48)},
        {BlockType.AcaciaLog, new BlockMesh(49, 55)},
        {BlockType.BirchLog, new BlockMesh(50, 56)},
        {BlockType.DarkOakLog, new BlockMesh(51, 57)},
        {BlockType.JungleLog, new BlockMesh(52, 58)},
        {BlockType.OakLog, new BlockMesh(53, 59)},
        {BlockType.SpruceLog, new BlockMesh(54, 60)},
        {BlockType.StrippedAcaciaLog, new BlockMesh(61, 67)},
        {BlockType.StrippedBirchLog, new BlockMesh(62, 68)},
        {BlockType.StrippedDarkOakLog, new BlockMesh(63, 69)},
        {BlockType.StrippedJungleLog, new BlockMesh(64, 70)},
        {BlockType.StrippedOakLog, new BlockMesh(65, 71)},
        {BlockType.StrippedSpruceLog, new BlockMesh(66, 72)},
        {BlockType.StrippedAcaciaWood, new BlockMesh(61)},
        {BlockType.StrippedBirchWood, new BlockMesh(62)},
        {BlockType.StrippedDarkOakWood, new BlockMesh(63)},
        {BlockType.StrippedJungleWood, new BlockMesh(64)},
        {BlockType.StrippedOakWood, new BlockMesh(65)},
        {BlockType.StrippedSpruceWood, new BlockMesh(66)},
        {BlockType.AcaciaLeaves, new BlockMesh(73, false)},
        {BlockType.BirchLeaves, new BlockMesh(74, false)},
        {BlockType.DarkOakLeaves, new BlockMesh(75, false)},
        {BlockType.JungleLeaves, new BlockMesh(76, false)},
        {BlockType.OakLeaves, new BlockMesh(77, false)},
        {BlockType.SpruceLeaves, new BlockMesh(78, false)},
        {BlockType.Glass, new BlockMesh(79)},
        {BlockType.BlackStainedGlass, new BlockMesh(80)},
        {BlockType.BlueStainedGlass, new BlockMesh(81)},
        {BlockType.BrownStainedGlass, new BlockMesh(82)},
        {BlockType.CyanStainedGlass, new BlockMesh(83)},
        {BlockType.GrayStainedGlass, new BlockMesh(84)},
        {BlockType.GreenStainedGlass, new BlockMesh(85)},
        {BlockType.LightBlueStainedGlass, new BlockMesh(86)},
        {BlockType.LightGrayStainedGlass, new BlockMesh(87)},
        {BlockType.LimeStainedGlass, new BlockMesh(88)},
        {BlockType.MagentaStainedGlass, new BlockMesh(89)},
        {BlockType.OrangeStainedGlass, new BlockMesh(90)},
        {BlockType.PinkStainedGlass, new BlockMesh(91)},
        {BlockType.PurpleStainedGlass, new BlockMesh(92)},
        {BlockType.RedStainedGlass, new BlockMesh(93)},
        {BlockType.WhiteStainedGlass, new BlockMesh(94)},
        {BlockType.YellowStainedGlass, new BlockMesh(95)},
        {BlockType.QuartzBlock, new BlockMesh(96)},
        {BlockType.SmoothQuartzBlock, new BlockMesh(97)},
        {BlockType.QuartzPillar, new BlockMesh(98, 99)},
        {BlockType.ChiseledQuartzBlock, new BlockMesh(100, 101)},
        {BlockType.QuartzBricks, new BlockMesh(102)}
        
    };
}