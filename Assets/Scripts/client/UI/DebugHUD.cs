using server.setting;
using UnityEngine;
using UnityEngine.UI;

namespace client.UI
{
    public class DebugHUD : MonoBehaviour
    {
        public Camera mainCamera;
        public Tweaks tweaks;
        public Text blockName;
        public GameObject player;

        public Text blockPosText;
        public Text speedText;
        public Text playerPosText;
        public Text resolutionText;

        private void Update()
        {
            ShowPlayerPosText();
            ShowSpeedText();
            ShowResolution();
            ShowBlockPosText();
        }

        private void ShowPlayerPosText()
        {
            Vector3 playerPos = player.transform.position;
            playerPos.y -= 0.625f;
            playerPosText.text = "Position: " +
                                 "<color=#C33C45>" + playerPos.x.ToString("0.000") + "</color> " +
                                 "<color=#23B56E>" + playerPos.y.ToString("0.000") + "</color> " +
                                 "<color=#1DA3D2>" + playerPos.z.ToString("0.000") + "</color>";
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

                Vector3 blockPos = hitInfo.transform.position;

                blockPosText.text = "< " + ((int) (blockPos.x - 0.5f)) + " , " +
                                    ((int) (blockPos.y + 0.5f)) + " , " +
                                    ((int) (blockPos.z - 0.5f)) + " >";
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
