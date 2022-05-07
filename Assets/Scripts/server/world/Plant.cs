using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Plant {
   private static readonly Tweaks Tweaks = Resources.Load<Tweaks>("ScriptableObjects/Tweaks");

   private static int TotalLength()
   {
      return 2 * Tweaks.chunkLength * Tweaks.viewDistance + Tweaks.chunkLength;
   }

   public static void Generation(int x, int y, int z)
   {
      if (!CanPlant(x, y - 1, z)) return;
      GrassGen(x, y, z);
      FlowersGen(x, y, z);
      TreesGen(x, y, z);
   }

   public static bool CanPlant(int x, int y, int z)
   {
      return Block.IsBlock(x, y, z, BlockType.GrassBlock) || Block.IsBlock(x, y, z, BlockType.Sand) ||
             Block.IsBlock(x, y, z, BlockType.Dirt);
   }
   
   private static void TreesGen(int x, int y, int z)
   {
      if (!CanPlant(x, y - 1, z)) return;
      if (Random.value < 0.01f && x > 2 && x < TotalLength() - 3 && z > 2 && z < TotalLength() - 3 && y < Tweaks.chunkHeight - 20)
      {
         BlockType treeType = (BlockType)Random.Range((int)BlockType.AcaciaLog, (int)BlockType.SpruceLog);
      
         switch (treeType)
         {
            case BlockType.AcaciaLog:
               AcaciaTreeGen(Random.Range(4, 10), x, y, z);
               break;
         
            case BlockType.BirchLog:
               NormalTreeGen(treeType, Random.Range(4, 8), x, y, z);
               break;
         
            case BlockType.DarkOakLog:
               break;
         
            case BlockType.JungleLog:
               NormalTreeGen(treeType, Random.Range(4, 15), x, y, z);
               break;
         
            case BlockType.OakLog:
               NormalTreeGen(treeType, Random.Range(4, 10), x, y, z);
               break;
         
            case BlockType.SpruceLog:
               int height = Random.value < 0.5f ? Random.Range(5, 10) : Random.Range(15, 30);
               SpruceTreeGen(height, x, y, z);
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }
      }
   }

   private static void NormalTreeGen(BlockType blockType, int height, int x, int y, int z)
   {
      // generate normal leaf shape for birch / oak / slim_jungle
      // 4 layers of leaves
      BlockType leafType = (BlockType) ((int) blockType + 18);
      for (int xL = -2; xL < 3; xL++)
      {
         for (int zL = -2; zL < 3; zL++)
         {
            if ((xL == 0 || zL == 0) && Math.Abs(xL) <= 1 && Math.Abs(zL) <= 1)
            {
               Block.SetBlock(x + xL, y + height, z + zL, leafType);     // top layer: 1
               Block.SetBlock(x + xL, y + height - 1, z + zL, leafType); // layer: 2
            }
            if (Math.Abs(xL) == 1 && Math.Abs(zL) == 1 && Random.value < 0.7f)
            {
               Block.SetBlock(x + xL, y + height - 1, z + zL, leafType); // corner leaf of layer 2
            } 
            if (Math.Abs(xL) < 2 || Math.Abs(zL) < 2) // layer: 3,4
            {
               Block.SetBlock(x + xL, y + height - 2, z + zL, leafType);
               Block.SetBlock(x + xL, y + height - 3, z + zL, leafType);
            }
            if (Math.Abs(xL) == 2 && Math.Abs(zL) == 2) // corner leaf of layer 3,4
            {
               if(Random.value < 0.4f) Block.SetBlock(x + xL, y + height - 2, z + zL, leafType);
               if(Random.value < 0.6f) Block.SetBlock(x + xL, y + height - 3, z + zL, leafType);
            }
         }
      }
   
      // generate the straight tree trunk
      for (int i = 0; i < height; i++)
      {
         Block.SetBlock(x, y + i, z, blockType);
      }
   }

   private static void AcaciaTreeGen(int height, int x, int y, int z)
   {
   
   }

   private static void SpruceTreeGen(int height, int x, int y, int z)
   {
   
   }

   private static void GrassGen(int x, int y, int z)
   {
      if (!CanPlant(x, y - 1, z)) return;
      if (Random.Range(1, 10) == 1 && x > 0 && x < TotalLength() - 1 && z > 0 && z < TotalLength() - 1 && y < Tweaks.chunkHeight - 2)
      {
         int grassType = Random.Range((int)BlockType.Grass, (int)BlockType.TallGrass);
         Block.SetBlock(x, y, z, (BlockType)grassType);
      }
   }

   private static void FlowersGen(int x, int y, int z)
   {
      if (!CanPlant(x, y - 1, z)) return;
      if (Random.Range(1, 20) == 1 && x > 0 && x < TotalLength() - 1 && z > 0 && z < TotalLength() - 1 && y < Tweaks.chunkHeight - 2)
      {
         int flowerType = Random.Range((int)BlockType.LilyOfTheValley, (int)BlockType.Peony);
         Block.SetBlock(x, y, z, (BlockType)flowerType);
      }
   }

}

