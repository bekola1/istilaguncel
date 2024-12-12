using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Hareket ve zıplama
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f; // Koşma hızı
    public float jumpForce = 5f;
    public float crouchHeight = 0.5f;

    // Zoom
    public float zoomFieldOfView = 30f;
    public float normalFieldOfView = 60f;
    public float zoomSpeed = 10f;

    // Mouse hareketi
    public float mouseSensitivity = 300f; // Artırılmış hassasiyet

    private Rigidbody rb;
    private Camera playerCamera;
    private Vector3 originalScale;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        originalScale = transform.localScale;

        // Mouse'u kilitle
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        Jump();
        Crouch();
        Zoom();
        LookAround();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal"); // A, D veya sağ-sol yön tuşları
        float moveZ = Input.GetAxis("Vertical");   // W, S veya ileri-geri yön tuşları

        bool isSprinting = Input.GetKey(KeyCode.LeftShift); // Sol Shift tuşuyla koşma
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        transform.Translate(move * currentSpeed * Time.deltaTime, Space.World);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl)) // Ctrl tuşu
        {
            transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
        }
        else
        {
            transform.localScale = originalScale;
        }
    }

    void Zoom()
    {
        if (Input.GetMouseButton(1)) // Sağ mouse tuşu
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFieldOfView, Time.deltaTime * zoomSpeed);
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFieldOfView, Time.deltaTime * zoomSpeed);
        }
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Kamera yukarı/aşağı hareketi (X ekseninde döner)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Baş yukarı/aşağı dönerken sınır koy

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Karakter sağ/sol dönüşü (Y ekseninde döner)
        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true; // Karakter yerde
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false; // Karakter havada
    }
}
