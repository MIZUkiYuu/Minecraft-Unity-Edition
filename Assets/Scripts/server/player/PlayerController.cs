using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Tweaks tweaks;
    public KeyBinding keyBinding;
    public Inventory inventory;
    public Camera mainCamera;
    public GameObject worldGen;
    public SoundsController soundsController;

    private Rigidbody _rigidbody;
    private WorldGen _world;
    // move
    private float _verticalVelocity;
    private Vector3 _tempMoveVelocity;
    private Vector3 _velocity;
    private float _moveSpeed; 
    // jump
    private bool _onGround;
    private bool _onCollisionStay;
    private Vector3 _jumpCheckBoxCenter;
    private readonly Vector3 _jumpCheckBoxHalfExtents= new(0.299f, 0.03f, 0.299f); //box : 0.6 0.06 0.6;
    // viewRotation
    private float _yRotation;
    // block placement
    private Ray _ray;
    private static bool _canRayCast;
    private static Vector3 _blockLookingPos;
    private static Vector3 _blockLookingPosInArray;
    private Vector2Int _chunkNum;
    private int _chunksInArray;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _world = worldGen.GetComponent<WorldGen>();
    }

    private void FixedUpdate()
    {
        Movement();
    }
    
    private void OnCollisionStay(Collision other) {
        _onCollisionStay = true;
    } 
    
    private void OnCollisionExit() {
        _onCollisionStay = false;
    }

    private void Update() {
        
        ViewRotation();
        BlockPlacement();
    }
    
    private void Movement() {
        if (Input.GetKey(keyBinding.sprint)) {
            _moveSpeed = 1.3f * 5.0f * tweaks.moveSpeed; // player sprint speed: 6.265
        }else{
            _moveSpeed = 5.0f * tweaks.moveSpeed; // player walk speed: 4.765
        }
        
        _velocity = Input.GetAxis("MoveZ") * transform.forward * _moveSpeed + Input.GetAxis("MoveX") * transform.right * _moveSpeed;

        if (!_onGround && _onCollisionStay) {
            _velocity.x = _velocity.z = 0;
        }
        
        _jumpCheckBoxCenter = transform.position - new Vector3(0, 1.6f, 0);
        _onGround = Physics.CheckBox(_jumpCheckBoxCenter, _jumpCheckBoxHalfExtents, Quaternion.Euler(Vector3.zero), LayerMask.GetMask("Block"));
        
        switch (_onGround) {
            case true:
                _velocity += 6f * Input.GetAxis("Jump") * Vector3.up * tweaks.maxJumpHeight;
                break;
            case false:
                _velocity.y += _rigidbody.velocity.y;
                break;
        }
        _rigidbody.velocity = _velocity;
        _velocity = Vector3.zero;
    }

    private void ViewRotation()
    {
        if(inventory.isGUIOpen)  return;
        
        float sensitivity = 1.5f * tweaks.sensitivity;
        float horizontalRotate = Input.GetAxis("Mouse X") * sensitivity * tweaks.horizontalSensitivity;
        float verticalRotate = Input.GetAxis("Mouse Y") * sensitivity * tweaks.verticalSensitivity;

        _yRotation -= verticalRotate;
        _yRotation = Mathf.Clamp(_yRotation, -90, 90);

        transform.Rotate(Vector3.up * horizontalRotate);
        mainCamera.transform.localRotation = Quaternion.Euler(_yRotation, 0, 0);
    }

    private void BlockPlacement() {
        if(inventory.isGUIOpen)  return;
        
        _ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
        if (Physics.Raycast(_ray, out RaycastHit hitInfo, tweaks.maxOperateDistance)) {
            _canRayCast = true;
            _blockLookingPos.x = hitInfo.normal.Equals(Vector3.right) ? Mathf.Floor(hitInfo.point.x) - 1 : Mathf.Floor(hitInfo.point.x);
            _blockLookingPos.y = Mathf.Clamp(hitInfo.normal.Equals(Vector3.up) ? Mathf.Floor(hitInfo.point.y) - 1 : Mathf.Floor(hitInfo.point.y), 0, tweaks.chunkHeight - 1);
            _blockLookingPos.z = hitInfo.normal.Equals(Vector3.forward) ? Mathf.Floor(hitInfo.point.z) - 1 : Mathf.Floor(hitInfo.point.z);
            _blockLookingPosInArray = _blockLookingPos + new Vector3(tweaks.viewDistance * tweaks.chunkLength, 0, tweaks.viewDistance * tweaks.chunkLength);
        }
        else {
            _canRayCast = false;
            return;
        }

        // place block
        if(Input.GetKeyDown(keyBinding.use) && CanPlaceBlock(_blockLookingPosInArray)){
            soundsController.PlayAudioClip(inventory.toolbar[inventory.toolbarSelectedItem], _blockLookingPos + hitInfo.normal, PlayerBehaviour.Place);
            Block.SetBlock(_blockLookingPosInArray + hitInfo.normal, inventory.toolbar[inventory.toolbarSelectedItem]);
            RefreshChunkMesh(_blockLookingPos + hitInfo.normal);
        }
        // dig block
        if (Input.GetKeyDown(keyBinding.attack)) {
            soundsController.PlayAudioClip(GetBlockLookingType(), _blockLookingPos, PlayerBehaviour.Dig);
            Block.SetBlock(_blockLookingPosInArray, BlockType.Air);
            RefreshChunkMesh(_blockLookingPos);
        }
    }

    private void RefreshChunkMesh(Vector3 blockPos) {
        _chunkNum = new Vector2Int((int)blockPos.x >> 4, (int)blockPos.z >> 4); // get the chunk number of x,y which target block is in
        _chunksInArray = (2 * tweaks.viewDistance + 1) * (_chunkNum.x + tweaks.viewDistance) + _chunkNum.y + tweaks.viewDistance;
        _world.chunks[_chunksInArray].numX = _chunkNum.x;
        _world.chunks[_chunksInArray].numZ = _chunkNum.y;
        _world.chunks[_chunksInArray].RefreshMesh(); // chunk array = 20 * (x + 10) + (y + 10)
    }

    private bool CanPlaceBlock(Vector3 blockPos) {
        if (Block.IsBlockInRange(inventory.toolbar[inventory.toolbarSelectedItem], Block.CanPlant)) {
            return Plant.CanPlant((int)blockPos.x, (int)blockPos.y, (int)blockPos.z);
        }
        return true;
    }
    
    public static Vector3 GetBlockLookingPos() {
        return _blockLookingPos;
    }

    public static BlockType GetBlockLookingType() {
        return Block.GetBlock(_blockLookingPosInArray);
    }
    
    public static bool CanRayCast() {
        return _canRayCast;
    }
    
}

public enum PlayerBehaviour{
    Dig, Place,
    Walk, Jump, Idle,
}
