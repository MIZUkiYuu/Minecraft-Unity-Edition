using System;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;

public class InventoryPanel : MonoBehaviour {
    public KeyBinding keyBinding;
    public Inventory inventory;
    [Space] 
    public GameObject slotsPanelScrollView;
    public GameObject normalBlockSlotsContent;
    public GameObject decorationSlotsContent;
    public GameObject normalBlockCategory;
    public GameObject decorationCategory;
    
    private GameObject _menuPanel;
    private Button _normalBlockCategoryBtn;
    private Button _decorationCategoryBtn;

    private static readonly Color GrayButton = new Color(0.6f, 0.6f, 0.6f, 0.7f);
    private static readonly Color WhiteButton = new Color(1f, 1f, 1f, 0.3f);

    private void Start() {
        normalBlockSlotsContent.SetActive(true);
        decorationSlotsContent.SetActive(true);
        
        _normalBlockCategoryBtn = normalBlockCategory.transform.GetChild(0).GetComponent<Button>();
        _decorationCategoryBtn = decorationCategory.transform.GetChild(0).GetComponent<Button>();
        _menuPanel = transform.GetChild(0).gameObject;

        // lock mouse cursor
        Cursor.lockState = CursorLockMode.Locked; 
        
        // generate textures of all blocks
        for (int i = 0; i < inventory.inventoryCreative[0].blockList.Count; i++) {
            normalBlockSlotsContent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<RawImage>().texture = ModelPreview.BlockTexture2Ds[inventory
            .GetBlockType(InventoryType.CreativeInventory, i, CategoryType.NormalBlock)];
        }
        for (int i = 0; i < inventory.inventoryCreative[1].blockList.Count; i++) {
            decorationSlotsContent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<RawImage>().texture = ModelPreview.BlockTexture2Ds[inventory
            .GetBlockType(InventoryType.CreativeInventory, i, CategoryType.Decoration)];
        }

        // set rawImage texture of categories
        normalBlockCategory.transform.GetChild(0).GetComponent<RawImage>().texture = ModelPreview.BlockTexture2Ds[BlockType.GrassBlock]; // grass_block
        decorationCategory.transform.GetChild(0).GetComponent<RawImage>().texture = ModelPreview.BlockTexture2Ds[BlockType.OxeyeDaisy];  // oxeye_daisy
        
        // select category
        SelectCategory(inventory.selectedCategory); 
        
        // default disable inventory menu panel
        _menuPanel.SetActive(false);
        inventory.isGUIOpen = false;
        
        // add button listener
        _normalBlockCategoryBtn.onClick.AddListener(() => SelectCategory(CategoryType.NormalBlock));
        _decorationCategoryBtn.onClick.AddListener(() => SelectCategory(CategoryType.Decoration));
    }

    private void Update() {
        if (!Input.GetKeyDown(keyBinding.inventory)) return;
        if (!inventory.isGUIOpen) {
            _menuPanel.SetActive(true);
            inventory.isGUIOpen = true;
            Cursor.lockState = CursorLockMode.None;  // unlock mouse cursor & Confine cursor to the game window
        }
        else {
            // reset mouse hold item
            inventory.isMouseHoldItem = false;
            inventory.mouseHoldItem = BlockType.Air;
            
            _menuPanel.SetActive(false);
            inventory.isGUIOpen = false;
            Cursor.lockState = CursorLockMode.Locked; // lock mouse cursor
        }
    }


    private void SelectCategory(CategoryType categoryType) {
        switch (categoryType) {
            case CategoryType.NormalBlock:
                inventory.selectedCategory = CategoryType.NormalBlock;
                normalBlockSlotsContent.SetActive(true);
                normalBlockCategory.GetComponent<Image>().color = GrayButton;
                decorationCategory.GetComponent<Image>().color = Color.clear;
                slotsPanelScrollView.GetComponent<ScrollRect>().content = normalBlockSlotsContent.GetComponent<RectTransform>();
                decorationSlotsContent.SetActive(false);
                break;
            case CategoryType.Decoration:
                inventory.selectedCategory = CategoryType.Decoration;
                decorationSlotsContent.SetActive(true);
                decorationCategory.GetComponent<Image>().color = GrayButton;
                normalBlockCategory.GetComponent<Image>().color = Color.clear;
                slotsPanelScrollView.GetComponent<ScrollRect>().content = decorationSlotsContent.GetComponent<RectTransform>();
                normalBlockSlotsContent.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(categoryType), categoryType, null);
        }
    }
    
}
