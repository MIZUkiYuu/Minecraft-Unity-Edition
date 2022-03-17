using System.Collections;
using server.setting;
using UnityEngine;
using UnityEngine.UI;

namespace client.UI.inventory
{
    public class ToolbarItemSelect : MonoBehaviour
    {
        public GameObject itemSelectFrame;
        public Text itemName;
        public Text posText;
        public Camera mainCamera;

        public Inventory inventory;
        public Tweaks tweaks;
        public KeyBinding keyBinding;

        Vector2 frameOriginPos;
        int frameCount = 1;

        private void Start()
        {
            //set texture to every item in toolbar
            int i = 1;
            foreach(GameObject item in inventory.toolbar)
            {
                GameObject.Find("toolbar_0" + i).GetComponent<RawImage>().texture = item.GetComponent<Renderer>().sharedMaterial.mainTexture;
                i++;
            }

            frameOriginPos = itemSelectFrame.transform.position;
        }

        private void Update()
        {

            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    frameCount = i + 1;
                    ShowText(itemName, frameCount, 0.5f);
                }
            }

            if (Input.mouseScrollDelta != Vector2.zero)
            {
                Vector2 mouseInput = Input.mouseScrollDelta;

                frameCount -= (int)mouseInput.y;
                if (frameCount > 9)
                {
                    frameCount = 1;
                }
                else if (frameCount < 1)
                {
                    frameCount = 9;
                }

                ShowText(itemName, frameCount, 0.5f);
            }

            if (Input.GetKeyDown(keyBinding.pick))
            {
                PickBlock();
                ShowText(itemName, frameCount, 0.5f);
            }
            //item_selector_frame move
            Vector2 framePos = itemSelectFrame.transform.position;
            framePos.x = frameOriginPos.x + 80 * (frameCount - 1);
            itemSelectFrame.transform.position = framePos;

            inventory.select_item = frameCount;

        }

        IEnumerator ShowItemName(Text text, int frameCount, float fadeTime)
        {
            Color aColor = text.color;
            aColor.a = 1.0f;
            text.color = aColor;
            text.text = inventory.toolbar[frameCount - 1].name;
            yield return new WaitForSeconds(2.0f);
            StartCoroutine(TextFade(text, fadeTime));
        }

        IEnumerator TextFade(Text text, float fadeTime)
        {
            for (float f = 1f; f >= 0; f -= 1 /(10 * fadeTime)) 
            {
                Color c = text.color;
                c.a = f;
                text.color = c;
                yield return new WaitForSeconds(.1f);
            }
        }

        void ShowText(Text text, int num, float fadeTime)
        {
            StopAllCoroutines();
            StartCoroutine(ShowItemName(text, num, fadeTime));
        }
        void PickBlock()
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            if(Physics.Raycast(ray,out RaycastHit hitInfo, tweaks.maxOperateDistance))
            {
                for(int i =0; i < inventory.toolbar.Length; i++)
                {
                    if(hitInfo.transform.name == inventory.toolbar[i].name + "(Clone)")
                    {
                        frameCount = i + 1;
                    }
                }
            }
        }

    }
}
