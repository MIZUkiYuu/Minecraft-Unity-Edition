using System;
using server.setting;
using UnityEngine;
using UnityEngine.UI;

namespace client.UI
{
    public class DebugHUD : MonoBehaviour
    {
        public Camera mainCamera;
        public Tweaks tweaks;
        public GameObject player;
        public Text blockName;
        
        [Space]
        public Text playerPosText;
        public Text chunkPosText;
        public Text speedText;
        public Text resolutionText;
        public Text blockPosText;

        private Vector3 _playerPos;

        private void Update()
        {
            _playerPos = player.transform.position;
            
            ShowPlayerPosText();
            ShowChunkPosText();
            ShowSpeedText();
            ShowResolution();
            ShowBlockPosText();
        }

        private void ShowPlayerPosText()
        {
            _playerPos.y -= 0.625f;
            playerPosText.text = $"{_playerPos.x:0.000} , {_playerPos.y:0.000} , {_playerPos.z:0.000}";
        }
        
        private void ShowChunkPosText() {
            Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt(_playerPos.x) >> 4, Mathf.FloorToInt(_playerPos.z) >> 4);
            chunkPosText.text = $"{chunkPos.x} , {chunkPos.y}";
        }

        private void ShowSpeedText()
        {
            Vector3 playerVc = player.GetComponent<Rigidbody>().velocity;
            speedText.text = "Speed: " + playerVc.magnitude.ToString("0.000") + "m/s" +
                             "  <color=#C33C45>" + playerVc.x.ToString("0.000") + "</color> " +
                             "<color=#23B56E>" + playerVc.y.ToString("0.000") + "</color> " +
                             "<color=#1DA3D2>" + playerVc.z.ToString("0.000") + "</color>";
        }

        private void ShowResolution()
        {
            resolutionText.text = "Resolution: " + Screen.width + " x " + Screen.height;
        }

        private void ShowBlockPosText()
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));

            if (Physics.Raycast(ray, out RaycastHit hitInfo, tweaks.maxOperateDistance))
            {
                blockPosText.gameObject.SetActive(true);
                blockName.gameObject.SetActive(true);

                Vector3 blockPos = hitInfo.point;

                blockPosText.text = $"< {Math.Ceiling(blockPos.x)} , {Math.Ceiling(blockPos.y)} , {Math.Ceiling(blockPos.z)} >";
                blockName.text = "Block: " + hitInfo.transform.gameObject.name.Replace("(Clone)", "");
            }
            else
            {
                blockPosText.gameObject.SetActive(false);
                blockName.gameObject.SetActive(false);
            }
        }
    }
}
