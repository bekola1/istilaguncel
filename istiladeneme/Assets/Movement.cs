using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;

    [SerializeField] private GameObject _ground; // Ground
    [SerializeField] private float _jumpForce = 3f;
    [SerializeField] private float _walkSpeed = 6f;
    [SerializeField] private float _distance = 0.4f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _minlimit;
    [SerializeField] private float _maxlimit;
    [SerializeField] private float _sensivity;
    [SerializeField] private LayerMask _mask; // Serializable
    private float _limit;
    private Camera _mainCam;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _mainCam = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_ground.transform.position, _distance, _mask);
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        float x_look = Input.GetAxisRaw("Mouse X");
        float Y_look = Input.GetAxisRaw("Mouse Y");
        _limit += Y_look;
        _limit = Mathf.Clamp(_limit, _minlimit, _maxlimit);
        _mainCam.transform.localEulerAngles =new Vector3(-_limit, 0, 0);
        transform.Rotate(transform.up * (x_look * _sensivity * Time.deltaTime));

        Vector3 direction = transform.right * x + transform.forward * y;
        _controller.Move(direction * _walkSpeed * Time.deltaTime);
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
        }

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
