using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugHUD : MonoBehaviour
{
    public Tweaks tweaks;
    public KeyBinding keyBinding;
    public GameObject player;

    [Space] 
    public GameObject debugText;
    public Text blockName;
    public Text playerPosText;
    public Text chunkPosText;
    public Text speedText;
    public Text resolutionText;
    public Text blockPosText;

    private Vector3 _playerPos;
    private Vector3 _blockPos;
    private Vector2Int _chunkPos;

    private void Start() {
        debugText.SetActive(false);
    }

    private void Update()
    {
        if (debugText.activeSelf) {
            _playerPos = player.transform.position;
            ShowPlayerPosText();
            ShowChunkPosText();
            ShowSpeedText();
            ShowResolution();
            if(Input.GetKeyDown(keyBinding.debugHUD))  debugText.SetActive(false); 
        }
        else {
            if(Input.GetKeyDown(keyBinding.debugHUD))  debugText.SetActive(true);
        }
        
        ShowBlockPosText();
    }

    private void ShowPlayerPosText()
    {
        _playerPos.y -= 0.625f;
        playerPosText.text = $"Position: {_playerPos.x:0.000} , {_playerPos.y:0.000} , {_playerPos.z:0.000}";
    }
    
    private void ShowChunkPosText() {
        _chunkPos = new Vector2Int(Mathf.FloorToInt(_playerPos.x) >> 4, Mathf.FloorToInt(_playerPos.z) >> 4);
        chunkPosText.text = $"Chunk: {_chunkPos.x} , {_chunkPos.y}";
    }

    private void ShowSpeedText()
    {
        Vector3 playerVc = player.GetComponent<Rigidbody>().velocity;
        speedText.text = $"Speed: {playerVc.magnitude:0.000}m/s " +
                         $"<color=#C33C45>{playerVc.x:0.000}</color> " +
                         $"<color=#23B56E>{playerVc.y:0.000}</color> " +
                         $"<color=#1DA3D2>{playerVc.z:0.000}</color>";
    }

    private void ShowResolution()
    {
        resolutionText.text = "Resolution: " + Screen.width + " x " + Screen.height;
    }

    private void ShowBlockPosText()
    {
        if (PlayerController.CanRayCast())
        {
            _blockPos = PlayerController.GetBlockLookingPos() + new Vector3(tweaks.viewDistance * tweaks.chunkLength, 0, tweaks.viewDistance * tweaks.chunkLength);
            blockPosText.text = $"< {_blockPos.x} , {_blockPos.y} , {_blockPos.z} >";
            blockName.text = "Block: " + Block.GetBlock(_blockPos);
            
            blockPosText.gameObject.SetActive(true);
            blockName.gameObject.SetActive(true);
        }
        else
        {
            blockPosText.gameObject.SetActive(false);
            blockName.gameObject.SetActive(false);
        }
    }
}

