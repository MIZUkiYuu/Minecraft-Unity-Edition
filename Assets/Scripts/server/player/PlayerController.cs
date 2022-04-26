using server.setting;
using UnityEngine;

namespace server.player
{
    public class PlayerController : MonoBehaviour
    {

        public Tweaks tweaks;
        public KeyBinding keyBinding;
        public Camera mainCamera;
        private Rigidbody _rigidbody;

        private float _yRotation;
        private bool _onGround;
        private Vector3 _jumpCheckBoxCenter;
        private Vector3 _jumpCheckBoxHalfExtents;
        private float _verticalVelocity;
        private Vector3[] _tempMoveVelocity = new Vector3[6];
        private Vector3 _velocity;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _jumpCheckBoxHalfExtents = new Vector3(0.299f, 0.03f, 0.299f);  //box : 0.6 0.2 0.6
        }

        private void Update()
        {
            Cursor.lockState = CursorLockMode.Locked; //mouse lock
            ViewRotation();
            Movement();
        }

        private void Movement()
        {
            if(!HasAnyMoveInput() || !_onGround){
                _velocity = Vector3.zero;
            }
            
            //Move
            Vector3[] moveVelocity = new Vector3[6];

            float moveSpeed = 215.85f * tweaks.moveSpeed * Time.fixedDeltaTime; //player walk speed: 4.317, 215.85
            if (Input.GetKey(keyBinding.sprint))
            {
                moveSpeed *= 1.3f; //player sprint speed: 5.612
            }

            if (_onGround)
            {
                if (Input.GetKey(keyBinding.moveForward)) moveVelocity[0] = transform.forward;
                if (Input.GetKeyUp(keyBinding.moveForward)) moveVelocity[0] = Vector3.zero;
                if (Input.GetKey(keyBinding.moveBack)) moveVelocity[1] = -transform.forward;
                if (Input.GetKeyUp(keyBinding.moveBack)) moveVelocity[1] = Vector3.zero;
                if (Input.GetKey(keyBinding.moveLeft)) moveVelocity[2] = -transform.right;
                if (Input.GetKeyUp(keyBinding.moveLeft)) moveVelocity[2] = Vector3.zero;
                if (Input.GetKey(keyBinding.moveRight)) moveVelocity[3] = transform.right;
                if (Input.GetKeyUp(keyBinding.moveRight)) moveVelocity[3] = Vector3.zero;

                foreach (Vector3 moveVc in moveVelocity)
                {
                    _velocity += moveVc;
                }
            }
            else
            {
                foreach (var tempMoveVc in _tempMoveVelocity)
                {
                    _velocity += tempMoveVc;
                }
            }
            _velocity.x = Mathf.Clamp(_velocity.x, -1, 1);
            _velocity.z = Mathf.Clamp(_velocity.z, -1, 1);

            _velocity.x *= moveSpeed;
            _velocity.z *= moveSpeed;
            
            //Jump
            _verticalVelocity -= 20 * tweaks.gravity * Time.deltaTime;

            _jumpCheckBoxCenter = transform.position - new Vector3(0, 1.6f, 0);
            _onGround = Physics.CheckBox(_jumpCheckBoxCenter, _jumpCheckBoxHalfExtents, Quaternion.Euler(Vector3.zero), LayerMask.GetMask("Block"));
            
            if (_onGround && _verticalVelocity < 0)
            {
                _verticalVelocity = 0;
            }
            _velocity += Vector3.up * (_verticalVelocity * Time.deltaTime);
            
            if (_onGround && Input.GetKeyDown(keyBinding.jump))
            {
                for (int i = 0; i < _tempMoveVelocity.Length; i++)
                {
                    _tempMoveVelocity[i] = moveVelocity[i];
                }
                _verticalVelocity = Mathf.Sqrt(400.0f * tweaks.gravity * tweaks.maxJumpHeight);
            }
            _rigidbody.velocity = _velocity;
        }

        private void ViewRotation()
        {
            float sensitivity = 200.0f * tweaks.sensitivity * Time.deltaTime;
            float horizontalRotate = Input.GetAxis("Mouse X") * sensitivity * tweaks.horizontalSensitivity;
            float verticalRotate = Input.GetAxis("Mouse Y") * sensitivity * tweaks.verticalSensitivity;

            _yRotation -= verticalRotate;
            _yRotation = Mathf.Clamp(_yRotation, -90, 90);

            transform.Rotate(Vector3.up * horizontalRotate);
            mainCamera.transform.localRotation = Quaternion.Euler(_yRotation, 0, 0);
        }

        private bool HasAnyMoveInput()
        {
            return Input.GetKey(keyBinding.moveForward) || Input.GetKey(keyBinding.moveBack) ||
                   Input.GetKey(keyBinding.moveLeft) || Input.GetKey(keyBinding.moveRight) ||
                   Input.GetKey(keyBinding.moveUp) || Input.GetKey(keyBinding.moveDown) || Input.GetKey(keyBinding.jump);
        }
    }
}