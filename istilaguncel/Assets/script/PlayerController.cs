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
    public Camera playerCamera; // Oyuncunun kamerası
    public GameObject gun; // Silah objesi
    private CharacterController characterController; // Karakterin fiziksel kontrolleri
    private Vector3 moveDirection; // Hareket yönü
    private float currentSpeed; // Şu anki hız
    private float defaultFOV; // Varsayılan FOV
    private float zoomedFOV = 30f; // Zoom yapıldığında uygulanacak FOV
    private float rotationX = 0f; // X ekseninde döndürme (yukarı ve aşağı)
    private bool isZooming = false; // Zoom yapılıp yapılmadığını kontrol et

    void Start()
    {
        characterController = GetComponent<CharacterController>(); // Karakter controller'ını al
        playerCamera = transform.GetChild(0).GetComponent<Camera>(); // Player objesinin altındaki ilk çocuğu (kamera) al
        defaultFOV = playerCamera.fieldOfView; // Varsayılan FOV'yu al
        currentSpeed = moveSpeed; // Başlangıçta normal hızda ilerle
        gun = transform.GetChild(1).gameObject; // Silahı almak için, silah karakterin altındaki 2. objeyi alıyoruz

        // Mouse imlecini gizle ve kilitle
        Cursor.lockState = CursorLockMode.Locked;  // İmleci kilitle
        Cursor.visible = false;  // İmleci gizle
    }

    void Update()
    {
        // Mouse hareketi ile yatay ve dikey kamerayı döndürme
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Y eksenindeki döndürmeyi sınırlayarak kamerayı yukarı aşağı hareket ettir

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f); // Kamerayı yukarı-aşağı döndür
        transform.Rotate(Vector3.up * mouseX); // Yalnızca oyuncuyu sağa-sola döndür

        // Koşma tuşuna basıldığında hızı artır
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed; // Koşma hızı
        }
        else
        {
            currentSpeed = moveSpeed; // Normal hız
        }

        // Yatay ve dikey hareket
        float moveDirectionY = moveDirection.y;
        float moveDirectionX = Input.GetAxis("Horizontal") * currentSpeed; // A ve D tuşları
        float moveDirectionZ = Input.GetAxis("Vertical") * currentSpeed; // W ve S tuşları

        // Hareket yönü (yerçekimi de dahil)
        moveDirection = new Vector3(moveDirectionX, moveDirectionY, moveDirectionZ);
        moveDirection = transform.TransformDirection(moveDirection);

        // Zıplama işlevi
        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y); // Zıplama hesaplaması
            }
        }
        else
        {
            moveDirection.y += Physics.gravity.y * Time.deltaTime; // Yerçekimi etkisi
        }

        characterController.Move(moveDirection * Time.deltaTime); // Hareketi uygula

        // FOV değişimi (koşarken)
        float targetFOV = Input.GetKey(KeyCode.LeftShift) ? defaultFOV + 20f : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fieldOfViewSpeed * Time.deltaTime);

        // Sağ tıklama ile zoom yapma
        if (Input.GetMouseButton(1)) // Sağ tık
        {
            isZooming = true;
        }
        else
        {
            isZooming = false;
        }

        // Zoom yapılırken FOV değerini değiştir
        float targetZoomFOV = isZooming ? zoomedFOV : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetZoomFOV, 10f * Time.deltaTime); // 10f hızla zoom yap

        // Silah Ateş Etme (Mouse sol tuşu ile)
        if (Input.GetMouseButton(0)) // Sol tıklama
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Silah ateşleme işlevi burada olacak.
        // Bu kısımda, kameranın yönüne göre bir raycast gönderebiliriz.
        
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition); // Kameradan farenin tıklama noktasına bir ray oluştur
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) // Ray, bir nesneye çarptıysa
        {
            if (hit.collider.CompareTag("Zombie")) // Eğer çarpan nesne zombi ise
            {
                hit.collider.GetComponent<ZombieHealth>().TakeDamage(10); // Zombi hasar alacak
            }
        }
    }
}
