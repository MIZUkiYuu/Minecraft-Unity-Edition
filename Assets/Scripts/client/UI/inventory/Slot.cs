using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public Inventory inventory;

    public RawImage movingImg;
    public bool isRemovable;
    
    private GameObject _slotParent;
    private Canvas _slotCanvas;
    private RawImage _slotImg;
    private TextMeshProUGUI _itemName;
    private int _slotNum;
    private BlockType _tempBlockType;

    private void Start() {
        movingImg.enabled = false;
        inventory.isInSlot = false;
        inventory.isMouseHoldItem = false;
        inventory.mouseHoldItem = BlockType.Air;
        
        _slotParent = transform.parent.gameObject;
        _slotCanvas = transform.GetChild(1).GetComponent<Canvas>();
        _slotImg = transform.GetChild(0).GetComponent<RawImage>();
        _itemName = _slotCanvas.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _itemName.text = _slotImg.texture.name;
        _slotParent.GetComponent<RawImage>().color = Color.clear;
        _slotCanvas.enabled = false;
    }

    private void Update() {
        if (inventory.isMouseHoldItem) {
            movingImg.enabled = true;
            movingImg.GetComponent<RectTransform>().position = Input.mousePosition;
            
            if(Input.GetMouseButtonUp(0) && !inventory.isInSlot) {
                inventory.isMouseHoldItem = false;
                inventory.mouseHoldItem = BlockType.Air;
                movingImg.texture = ModelPreview.BlockTexture2Ds[BlockType.Air];
                movingImg.enabled = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _slotParent.GetComponent<RawImage>().color = new Color(0.85f, 0.85f, 0.85f, 0.8f);
        _slotCanvas.enabled = true;
        _itemName.text = _slotImg.texture.name;
        _slotCanvas.transform.GetChild(0).gameObject.SetActive(GetBlockTypeFromSlot(eventData) != BlockType.Air);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_slotCanvas.GetComponent<RectTransform>());
        inventory.isInSlot = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        _slotParent.GetComponent<RawImage>().color = Color.clear;
        _slotCanvas.enabled = false;
        inventory.isInSlot = false;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!inventory.isMouseHoldItem) {
            if (isRemovable) { 
                if (GetBlockTypeFromSlot(eventData) == BlockType.Air) {
                    inventory.isMouseHoldItem = false;
                }
                else {
                    inventory.isMouseHoldItem = true;
                    inventory.mouseHoldItem = GetBlockTypeFromSlot(eventData);
                    movingImg.texture = ModelPreview.BlockTexture2Ds[inventory.mouseHoldItem];
                    _slotImg.texture = ModelPreview.BlockTexture2Ds[BlockType.Air];
                }
            }
            else {
                inventory.isMouseHoldItem = true;
                inventory.mouseHoldItem = GetBlockTypeFromSlot(eventData);
                movingImg.texture = ModelPreview.BlockTexture2Ds[inventory.mouseHoldItem];
            }
        }
        else {
            if (!isRemovable) {  
                inventory.mouseHoldItem = GetBlockTypeFromSlot(eventData);
                movingImg.texture = ModelPreview.BlockTexture2Ds[GetBlockTypeFromSlot(eventData)];
            }
            else {
                if (GetBlockTypeFromSlot(eventData) == BlockType.Air) {
                    inventory.isMouseHoldItem = false;
                    
                    movingImg.texture = ModelPreview.BlockTexture2Ds[BlockType.Air];
                    
                    inventory.SetBlockType(InventoryType.Toolbar, inventory.mouseHoldItem, GetNum());
                    _slotImg.texture = ModelPreview.BlockTexture2Ds[inventory.mouseHoldItem];
                    
                    inventory.mouseHoldItem = BlockType.Air;
                    movingImg.enabled = false;
                }
                else {
                    _tempBlockType = GetBlockTypeFromSlot(eventData);
                    
                    inventory.SetBlockType(InventoryType.Toolbar, inventory.mouseHoldItem, GetNum());
                    _slotImg.texture = ModelPreview.BlockTexture2Ds[inventory.mouseHoldItem];
                    
                    inventory.mouseHoldItem = _tempBlockType;
                    movingImg.texture = ModelPreview.BlockTexture2Ds[_tempBlockType];
                }
                LayoutRebuilder.ForceRebuildLayoutImmediate(_slotCanvas.GetComponent<RectTransform>());
            }
        }
    }

    private BlockType GetBlockTypeFromSlot(PointerEventData eventData) {
        return (BlockType) Enum.Parse(typeof(BlockType), eventData.pointerCurrentRaycast.gameObject.GetComponent<RawImage>().texture.name);
    }
    
    private int GetNum() {
        return int.Parse(System.Text.RegularExpressions.Regex.Replace(_slotParent.name, @"[^0-9]+", ""));
    }

}
