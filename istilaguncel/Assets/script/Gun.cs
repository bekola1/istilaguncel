using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int damage = 20; // Silahın vereceği hasar
    public float fireRate = 0.5f; // Ateş etme hızı (saniye başına)
    public float range = 100f; // Silahın menzili
    private float nextTimeToFire = 0f; // Sonraki ateş etme zamanı
    public Camera playerCamera; // Oyuncunun kamerası
    public GameObject bulletPrefab; // Mermi prefab'ı
    public Transform bulletSpawnPoint; // Merminin çıkacağı nokta

    void Start()
    {
        // Eğer playerCamera atanmadıysa, ana kamerayı al
        if (playerCamera == null)
            playerCamera = Camera.main; // Kamera atanmazsa ana kamerayı al

        if (playerCamera == null)
        {
            Debug.LogError("Camera referansı atanmadı! Lütfen kamerayı 'playerCamera' alanına atayın.");
        }
    }

    void Update()
    {
        // Sol fare tuşu ile ateş etme (0 = sol fare tuşu)
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate; // Bir sonraki ateş etme zamanını ayarla
            Shoot(); // Ateş et
        }
    }

    void Shoot()
    {
        RaycastHit hit; // Çarpışma bilgilerini tutacak değişken

        // Oyuncunun kameradan ileriye doğru ateş etme
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log("Ateş edildi ve bir hedefe çarptı!");

            // Çarpışan objenin ZombieController scripti varsa, hasar ver
            ZombieController zombieController = hit.collider.GetComponent<ZombieController>();
            if (zombieController != null)
            {
                zombieController.TakeDamage(damage); // Zombiye hasar ver
            }
        }

        // Mermi prefab'ı oluşturuluyor
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            // Yeni bir mermi instantiate et (oluştur)
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            
            // Mermiyi hızlandırmak için Rigidbody bileşenini al
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(playerCamera.transform.forward * 1000f); // Mermiyi ileriye doğru hareket ettir
            }
        }
    }
}
