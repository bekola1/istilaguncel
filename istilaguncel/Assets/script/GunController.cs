using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;  // Mermi prefab'ı
    public Transform firePoint;      // Ateş etme noktası
    public float fireForce = 1000f;  // Ateş gücü

    private void Update()
    {
        // Ateş etme işlemi için sağ tıklamayı kontrol et
        if (Input.GetMouseButtonDown(0)) // Sol fare tuşuna basıldığında
        {
            Fire();
        }
    }

    void Fire()
    {
        // Mermiyi instantiate et
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            // Bullet oluşturulmuşsa, onu işleme al
            if (bullet != null)
            {
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                if (bulletRb != null)
                {
                    bulletRb.AddForce(firePoint.forward * fireForce); // Mermiye kuvvet uygula
                }
            }
            else
            {
                Debug.LogError("Bullet prefab is missing or invalid!");
            }
        }
        else
        {
            Debug.LogError("Bullet prefab or fire point is not assigned!");
        }
    }
}
