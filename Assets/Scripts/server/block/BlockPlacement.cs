using System;
using server;
using server.setting;
using UnityEngine;

public class BlockPlacement : MonoBehaviour
{
    public Camera mainCamera;

    public KeyBinding keyBinding;
    public Tweaks tweaks;
    public Inventory inventory;
    private TagList _tagList;
    private GameObject _block;

    private readonly Vector3 _screenCenterPoint = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0);
    private LayerMask _mask;

    #region Singleton
    private void Awake()
    {
        _tagList = new TagList();
    }

    private void Start()
    {
        _mask = LayerMask.GetMask("Block");
    }

    #endregion

    private void Update()
    {

        if (Input.GetKeyDown(keyBinding.use))
        {
            _block = inventory.toolbar[inventory.select_item - 1];
            SetBlock(transform.position, _block);
        }
        if (Input.GetKeyDown(keyBinding.attack))    
        {
            RemoveBlock(transform.position);
        }

    }


    void SetBlock(Vector3 playerPos, GameObject block)
    {
        Ray ray = mainCamera.ScreenPointToRay(_screenCenterPoint); // screen center
        if (Physics.Raycast(ray, out RaycastHit hitInfo, tweaks.maxOperateDistance, _mask) && WithinOperateRange(playerPos, hitInfo)) {
            Vector3 blockPos = hitInfo.collider.transform.position;
            PlaceBlock(blockPos, block, hitInfo);
        }
    }

    void RemoveBlock(Vector3 playerPos)
    {
        Ray ray = mainCamera.ScreenPointToRay(_screenCenterPoint); // screen center
        if (Physics.Raycast(ray, out RaycastHit hitInfo, tweaks.maxOperateDistance, _mask))
        {
            Vector3 blockPos = hitInfo.collider.transform.position;
            if (WithinOperateRange(playerPos, hitInfo) && hitInfo.collider.CompareTag(_tagList.Block))
            {
                Destroy(hitInfo.collider.gameObject);
            }
        }
        
    }


    public void ReplaceBlock()
    {

    }

    void PlaceBlock(Vector3 blockPos, GameObject gameObject, RaycastHit hitInfo)
    {
        blockPos.x = Mathf.Floor(blockPos.x) + 0.5f;
        blockPos.y = Mathf.Floor(blockPos.y) + 0.5f;
        blockPos.z = Mathf.Floor(blockPos.z) + 0.5f;
        
        Instantiate(gameObject, blockPos + hitInfo.normal, Quaternion.identity);
    }

    Boolean WithinOperateRange(Vector3 playerPos,RaycastHit hitInfo)
    {
        Vector3 blockPos = hitInfo.collider.transform.position;
        return (playerPos - blockPos).sqrMagnitude <= Mathf.Pow(tweaks.maxOperateDistance, 2);
    }
    
}
