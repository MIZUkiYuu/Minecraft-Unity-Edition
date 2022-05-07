using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Toolbar : MonoBehaviour {
    public Inventory inventory;
    public KeyBinding keyBinding;
    [Space]
    public Transform toolbarSlot;
    public Text itemName;
    public RectTransform itemSelectFrame;
    
    private float _frameOriginPos;
    private int _frameCount;
    private const float FadeTime = 0.5f;

    private void Awake() {
        inventory.mouseHoldItem = BlockType.Air;
        //set texture to every item in toolbar
        for (int i = 0; i < inventory.toolbar.Length; i++) {
            toolbarSlot.GetChild(i).GetChild(0).GetChild(0).GetComponent<RawImage>().texture = ModelPreview.BlockTexture2Ds[inventory.GetBlockType
            (InventoryType.Toolbar, i)];
        }
        _frameOriginPos = itemSelectFrame.anchoredPosition.x;
    }

    private void Update() {
        if(inventory.isGUIOpen) return;
        // press key 1-9 to select block
        for (int i = 1; i <= 10; i++) {
            if (!Input.GetKeyDown((i - 1).ToString())) continue;
            _frameCount = i - 2;
            ShowText();
        }
        // use mouse scroller to select block
        if (Input.mouseScrollDelta != Vector2.zero) {
            Vector2 mouseInput = Input.mouseScrollDelta;

            _frameCount -= (int)mouseInput.y;
            _frameCount = _frameCount switch {
                > 8 => 0,
                < 0 => 8,
                _ => _frameCount
            };
            ShowText();
        }
        // press mouse middle key to select block
        if (Input.GetKeyDown(keyBinding.pick)) {
            inventory.SetBlockType(InventoryType.Toolbar, PlayerController.GetBlockLookingType(), inventory.toolbarSelectedItem);
            toolbarSlot.GetChild(inventory.toolbarSelectedItem).GetChild(0).GetChild(0).GetComponent<RawImage>().texture = ModelPreview.BlockTexture2Ds[inventory.GetBlockType(InventoryType.Toolbar)];
            ShowText();
        }

        if (Input.GetKeyDown(keyBinding.dropItem)) {
            inventory.SetBlockType(InventoryType.Toolbar, BlockType.Air, inventory.toolbarSelectedItem);
            toolbarSlot.GetChild(inventory.toolbarSelectedItem).GetChild(0).GetChild(0).GetComponent<RawImage>().texture = ModelPreview.BlockTexture2Ds[BlockType.Air];
            StopAllCoroutines();
        }
        //item_selector_frame move
        Vector2 framePos = itemSelectFrame.anchoredPosition;
        framePos.x = _frameOriginPos + 80 * _frameCount;
        itemSelectFrame.anchoredPosition = framePos;

        inventory.toolbarSelectedItem = _frameCount;

    }

    private IEnumerator ShowItemName() {
        itemName.text = inventory.toolbar[_frameCount].ToString();
        Color aColor = itemName.color;
        aColor.a = 1.0f;
        itemName.color = aColor;
        
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(TextFade(itemName));
    }

    private IEnumerator TextFade(Text text) {
        for (float f = 1f; f > -.1f; f -= 0.1f) {
            Color c = text.color;
            c.a = f;
            text.color = c;
            yield return new WaitForSeconds(FadeTime / 10);
        }
    }

    private void ShowText() {
        StopAllCoroutines();
        itemName.text = "";
        if(inventory.toolbar[_frameCount].ToString().Equals("Air")) return;
        StartCoroutine(ShowItemName());
    }
}

