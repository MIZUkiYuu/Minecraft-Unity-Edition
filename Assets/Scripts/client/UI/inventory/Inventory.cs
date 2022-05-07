using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory")]

public class Inventory : ScriptableObject
{
    [Header("---- Toolbar ----")]
    public int toolbarSelectedItem;
    public BlockType[] toolbar;

    [Header("---- InventoryMenu ----")] 
    public bool isGUIOpen = false;
    public bool isInSlot = false;
    public bool isMouseHoldItem = false;
    public BlockType mouseHoldItem = BlockType.Air;
    public CategoryType selectedCategory;
    public List<InventoryCategory> inventoryCreative = new();
    public List<BlockType> inventorySurvival = new();

    public void SetBlockType( InventoryType inventoryType, BlockType blockType, int num = 0, CategoryType categoryType = CategoryType.NormalBlock) {
        switch (inventoryType) {
            case InventoryType.Toolbar:
                toolbar[num] = blockType;
                break;
            
            case InventoryType.CreativeInventory:
                inventoryCreative[(int)categoryType].blockList[num] = blockType;
                break;

            case InventoryType.SurvivalInventory:
                inventorySurvival[num] = blockType;
                break;

            case InventoryType.Categorybar:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(inventoryType), inventoryType, null);
        }
    }
    
    public BlockType GetBlockType(InventoryType inventoryType, int num = 0, CategoryType categoryType = CategoryType.NormalBlock) {
        switch (inventoryType) {
            case InventoryType.Toolbar:
                return toolbar[num];
            
            case InventoryType.CreativeInventory:
                return inventoryCreative[(int)categoryType].blockList[num];

            case InventoryType.SurvivalInventory:
                return inventorySurvival[num];

            case InventoryType.Categorybar:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(inventoryType), inventoryType, null);
        }
        return BlockType.Air;
    }
}

public enum InventoryType{
    Toolbar, CreativeInventory, SurvivalInventory, Categorybar
}

public enum CategoryType {
    NormalBlock, Decoration
}