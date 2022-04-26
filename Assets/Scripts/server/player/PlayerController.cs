using System;
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
        private Vector3 _tempMoveVelocity;
        private Vector3 _velocity;
        private float _moveSpeed; 
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _jumpCheckBoxHalfExtents = new Vector3(0.299f, 0.03f, 0.299f);  //box : 0.6 0.2 0.6
        }

        
        private void FixedUpdate()
        {
            Movement();
        }

        private void Update() {
            Cursor.lockState = CursorLockMode.Locked; //mouse lock
            ViewRotation();
        }

        private void Movement() {
            //Move

            if (Input.GetKey(keyBinding.sprint)) {
                _moveSpeed = 1.3f * 5.0f * tweaks.moveSpeed; // player sprint speed: 6.265
            }else{
                _moveSpeed = 5.0f * tweaks.moveSpeed; // player walk speed: 4.765
            }
            
            _velocity = Input.GetAxis("MoveZ") * transform.forward * _moveSpeed + Input.GetAxis("MoveX") * transform.right * _moveSpeed;

            _jumpCheckBoxCenter = transform.position - new Vector3(0, 1.6f, 0);
            _onGround = Physics.CheckBox(_jumpCheckBoxCenter, _jumpCheckBoxHalfExtents, Quaternion.Euler(Vector3.zero), LayerMask.GetMask("Block"));
            
            switch (_onGround) {
                case true:
                    _velocity += 4f * Input.GetAxis("Jump") * Vector3.up * tweaks.maxJumpHeight;
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
            float sensitivity = 2.0f * tweaks.sensitivity;
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