using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI elemanları için gerekli

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;  // Mermi prefab'ı
    public Transform firePoint;      // Ateş etme noktası
    public float fireForce = 1000f;  // Ateş gücü
    public int maxAmmo = 240;        // Maksimum toplam mermi sayısı
    public int magazineSize = 30;    // Şarjör boyutu
    private int currentAmmo;         // Mevcut toplam mermi sayısı
    private int currentMagazine;     // Mevcut şarjör mermi sayısı
    public float reloadTime = 2f;    // Yeniden doldurma süresi
    private bool isReloading = false;
    public Text ammoText;            // Mermi sayısını gösterecek UI elemanı

    private void Start()
    {
        currentAmmo = maxAmmo;       // Başlangıçta maksimum toplam mermi sayısı
        currentMagazine = magazineSize; // Başlangıçta dolu şarjör

        // Sağ alt köşeye yerleştirmek için RectTransform ayarlarını yap
        RectTransform rectTransform = ammoText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 0);  // Sağ alt köşe
        rectTransform.anchorMax = new Vector2(1, 0);  // Sağ alt köşe
        rectTransform.pivot = new Vector2(1, 0);      // Pivot noktası sağ alt köşe
        rectTransform.anchoredPosition = new Vector2(-10, 10);  // Sağ alt köşeden 10 birim içeride

        UpdateAmmoUI();              // UI'ı güncelle
    }

    private void Update()
    {
        if (isReloading)
            return;

        if (currentMagazine <= 0)
        {
            if (currentAmmo > 0)
            {
                StartCoroutine(Reload());
            }
            return;
        }

        // Ateş etme işlemi için sol tıklamayı kontrol et
        if (Input.GetMouseButtonDown(0) && currentMagazine > 0) // Sol fare tuşuna basıldığında
        {
            Fire();
        }

        // Manuel yeniden doldurma işlemi
        if (Input.GetKeyDown(KeyCode.R) && currentMagazine < magazineSize && currentAmmo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);

        int ammoToReload = magazineSize - currentMagazine;
        if (currentAmmo >= ammoToReload)
        {
            currentAmmo -= ammoToReload;
            currentMagazine = magazineSize;
        }
        else
        {
            currentMagazine += currentAmmo;
            currentAmmo = 0;
        }

        isReloading = false;
        UpdateAmmoUI(); // Yeniden doldurma sonrası UI'ı güncelle
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

            currentMagazine--; // Şarjör mermi sayısını azalt
            UpdateAmmoUI();  // UI'ı güncelle
        }
        else
        {
            Debug.LogError("Bullet prefab or fire point is not assigned!");
        }
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentMagazine + "/" + currentAmmo;
        }
    }
}