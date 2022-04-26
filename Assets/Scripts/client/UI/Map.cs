using System;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour {
    public KeyBinding keyBinding;
    [Space] 
    public Image playerHead;
    public Camera mapCamera;
    public RenderTexture minimapRT;
    public RenderTexture worldmapRT;
    public GameObject minimap;
    public GameObject worldmap;

    private RectTransform _playerIcon;
    private void Start() {
        _playerIcon = playerHead.GetComponent<RectTransform>();
    }

    private void Update() {
        if (!Input.GetKeyDown(keyBinding.worldmap)) return;
        if (!worldmap.activeSelf) {
            minimap.SetActive(false);
            mapCamera.orthographicSize = 60;
            mapCamera.targetTexture = worldmapRT;
            _playerIcon.sizeDelta = new Vector2(4, 4);
            worldmap.SetActive(true);
        }
        else {
            worldmap.SetActive(false);
            mapCamera.orthographicSize = 50;
            mapCamera.targetTexture = minimapRT;
            _playerIcon.sizeDelta = new Vector2(8, 8);
            minimap.SetActive(true);
        }
    }
}
