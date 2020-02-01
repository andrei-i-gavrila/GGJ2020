using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace GGJ
{
    public class MoveController : MonoBehaviour
    {
        public Camera Camera;
        public float MouseSensitivity = 10f;
        public float BaseSpeed = 5f;
        public float RunningMultiplier = 1.5f;
        private float _pitch;
        private Rigidbody _rb;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            CameraLook();
            BodyRotation();
            BodyMovement();
        }

        private void BodyMovement()
        {
            var speed = BaseSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed *= RunningMultiplier;
            }

            var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized * speed;
            movement = transform.TransformDirection(movement);


            _rb.velocity = movement;
        }

        private void BodyRotation()
        {
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * MouseSensitivity);
        }

        private void CameraLook()
        {
            _pitch -= Input.GetAxis("Mouse Y") * MouseSensitivity;
            _pitch = Mathf.Clamp(_pitch, -90, 90);
            Camera.transform.localEulerAngles = new Vector3(_pitch, 0f, 0f);
        }
    }
}