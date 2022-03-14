using System;
using server.setting;
using UnityEngine;

namespace server.player
{
    public class PlayerManager : MonoBehaviour
    {

        public Tweaks tweaks;
        public KeyBinding keyBinding;
        public Camera mainCamera;
        
        private Rigidbody _rigidbody;

        private float _yRotation;
        //jump
        private Vector3 _velocity;
        private bool _onGround;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.transform.position.y + 1.625f - transform.position.y >= 0)
            {
                _onGround = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.transform.position.y + 1.625f < transform.position.y)
            {
                _onGround = false;
                Debug.Log(_onGround);
            }
        }

        private void Update()
        {
            Cursor.lockState = CursorLockMode.Locked; //mouse lock
            ViewRotation();
            Move();
            Jump();
        }

     
        private void Move()
        {
            _rigidbody.velocity = Vector3.zero;
            if (Input.GetKey(keyBinding.moveForward))    _rigidbody.velocity += transform.forward;
            if (Input.GetKeyUp(keyBinding.moveForward))    _rigidbody.velocity -= transform.forward;
            if (Input.GetKey(keyBinding.moveBack))    _rigidbody.velocity += -transform.forward;
            if (Input.GetKeyUp(keyBinding.moveBack))    _rigidbody.velocity += transform.forward;
            if (Input.GetKey(keyBinding.moveLeft))    _rigidbody.velocity += -transform.right;
            if (Input.GetKeyUp(keyBinding.moveLeft))    _rigidbody.velocity += transform.right;
            if (Input.GetKey(keyBinding.moveRight))    _rigidbody.velocity += transform.right;
            if (Input.GetKeyUp(keyBinding.moveRight))    _rigidbody.velocity -= transform.right;
            // if (Input.GetKey(keyBinding.moveUp))    _rigidbody.velocity += transform.up;
            // if (Input.GetKeyUp(keyBinding.moveUp))    _rigidbody.velocity += Vector3.up;
            // if (Input.GetKey(keyBinding.moveDown))    _rigidbody.velocity += -transform.up;
            // if (Input.GetKeyUp(keyBinding.moveDown))    _rigidbody.velocity += Vector3.down;
            
            // _rigidbody.velocity.M
            
            float moveSpeed = 215.85f * tweaks.moveSpeed * Time.fixedDeltaTime;     //player walk speed: 4.317
            if (Input.GetKey(keyBinding.sprint))
            {
                moveSpeed *= 1.3f;      //player sprint speed: 5.612
            }
            _rigidbody.velocity *= moveSpeed;
        }

        private void ViewRotation()
        {
            float sensitivity = 400.0f * tweaks.sensitivity * Time.deltaTime;
            float horizontalRotate = Input.GetAxis("Mouse X") * sensitivity * tweaks.horizontalSensitivity;
            float verticalRotate = Input.GetAxis("Mouse Y") * sensitivity * tweaks.verticalSensitivity;

            _yRotation -= verticalRotate;
            _yRotation = Mathf.Clamp(_yRotation, -90, 90);

            transform.Rotate(Vector3.up * horizontalRotate);
            mainCamera.transform.localRotation = Quaternion.Euler(_yRotation, 0, 0);
        }

        private void Jump()
        {
            if (_onGround && _velocity.y < 0)
            {
                _velocity.y = 0;
            }
            
            if (Input.GetKeyDown(keyBinding.jump) && _onGround)
            {
                _velocity.y = tweaks.jumpForce;
                Debug.Log("1");
                _velocity.y -= tweaks.gravity * Time.deltaTime;
                _rigidbody.velocity += _velocity * Time.deltaTime;
            }
        }

    }
}