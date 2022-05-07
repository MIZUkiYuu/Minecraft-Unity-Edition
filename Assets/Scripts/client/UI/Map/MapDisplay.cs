using UnityEngine;
using UnityEngine.UI;

public class MapDisplay : MonoBehaviour {
    public KeyBinding keyBinding;
    [Space] 
    public Camera mapCamera;
    public GameObject player;
    public RectTransform mapInfo;
    public RenderTexture minimapRT;
    public RenderTexture worldmapRT;
    public GameObject minimap;
    public GameObject worldmap;
    public Text chunkPosText;
    public Text playerPosText;

    private Vector3 _playerPos;
    private void Start() {
        ShowMiniMap();
    }

    private void Update() {
        _playerPos = player.transform.position;
        ShowChunkPosText();
        ShowPlayerPosText();
        
        // toggle minimap or worldmap
        if (!Input.GetKeyDown(keyBinding.worldmap)) return;
        if (!worldmap.activeSelf) {
            ShowWorldMap();
        }
        else {
            ShowMiniMap();
        }
    }

    private void ShowWorldMap() {
        minimap.SetActive(false);
        mapCamera.orthographicSize = 100;
        mapCamera.targetTexture = worldmapRT;
        mapInfo.anchoredPosition = new Vector2(0 , 690);
        mapInfo.sizeDelta = new Vector2(700, 100);
        worldmap.SetActive(true);
    }

    private void ShowMiniMap() {
        worldmap.SetActive(false);
        mapCamera.orthographicSize = 50;
        mapCamera.targetTexture = minimapRT;
        mapInfo.anchoredPosition = new Vector2(0 , 290);
        mapInfo.sizeDelta = new Vector2(300, 100);
        minimap.SetActive(true);
    }
    
    private void ShowChunkPosText() {
        chunkPosText.text = $"{Mathf.FloorToInt(_playerPos.x) >> 4} , {Mathf.FloorToInt(_playerPos.z) >> 4}";
    }

    private void ShowPlayerPosText()
    {
        _playerPos.y -= 0.625f;
        playerPosText.text = $"{(_playerPos.x - 0.5f):0.0} , {_playerPos.y:0.0} , {(_playerPos.z - 0.5f):0.0}";
    }
}
