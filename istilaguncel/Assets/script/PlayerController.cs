using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // Karakter controller'ının mevcut olduğundan emin ol
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Yürüyüş hızı
    public float sprintSpeed = 10f; // Koşma hızı
    public float jumpHeight = 2f; // Zıplama yüksekliği
    public float mouseSensitivity = 2f; // Mouse hassasiyeti
    public float fieldOfViewSpeed = 10f; // FOV değişim hızı
    public float zoomedFOV = 30f; // Zoom yapıldığında uygulanacak FOV
    private float defaultFOV; // Varsayılan FOV
    private float rotationX = 0f; // X ekseninde döndürme (yukarı ve aşağı)
    private bool isZooming = false; // Zoom yapılıp yapılmadığını kontrol et

    public Camera playerCamera; // Oyuncunun kamerası
    public GameObject gun; // Silah objesi

    private CharacterController characterController; // Karakterin fiziksel kontrolleri
    private Vector3 moveDirection; // Hareket yönü
    private float currentSpeed; // Şu anki hız

    void Start()
    {
        // CharacterController bileşenini al
        characterController = GetComponent<CharacterController>();

        // Kamera bileşenini dinamik olarak bul
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("Player camera not found! Ensure a Camera exists as a child of the Player object.");
        }

        // Silahı bulmaya çalış
        if (playerCamera != null)
        {
            gun = playerCamera.transform.Find("Gun")?.gameObject;
            if (gun == null)
            {
                Debug.LogError("Gun object not found! Ensure the Gun is a child of the Camera and named 'Gun'.");
            }
        }

        // Varsayılan ayarları yap
        defaultFOV = playerCamera != null ? playerCamera.fieldOfView : 60f;
        currentSpeed = moveSpeed;

        // Mouse imlecini gizle ve kilitle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleZoom();
        HandleShooting();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float moveDirectionY = moveDirection.y;
        float moveDirectionX = Input.GetAxis("Horizontal") * currentSpeed;
        float moveDirectionZ = Input.GetAxis("Vertical") * currentSpeed;

        moveDirection = new Vector3(moveDirectionX, moveDirectionY, moveDirectionZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            }
        }
        else
        {
            moveDirection.y += Physics.gravity.y * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
    }

    void HandleZoom()
    {
        if (Input.GetMouseButton(1)) // Sağ tık
        {
            isZooming = true;
        }
        else
        {
            isZooming = false;
        }

        float targetFOV = isZooming ? zoomedFOV : defaultFOV;
        if (playerCamera != null)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fieldOfViewSpeed * Time.deltaTime);
        }
    }

    void HandleShooting()
    {
        if (Input.GetMouseButton(0)) // Sol tıklama
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (gun == null || playerCamera == null)
        {
            Debug.LogWarning("Cannot shoot: Gun or Camera is not assigned.");
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Zombie"))
            {
                ZombieHealth zombieHealth = hit.collider.GetComponent<ZombieHealth>();
                if (zombieHealth != null)
                {
                    zombieHealth.TakeDamage(10);
                }
            }
        }
    }
}
