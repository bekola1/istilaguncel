using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maksimum sağlık
    public int currentHealth; // Şu anki sağlık

    // Başlangıç ayarları
    void Start()
    {
        currentHealth = maxHealth; // Oyuncunun başlangıç sağlığı
    }

    // Sağlık alma fonksiyonu
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Hasar alındığında sağlık azaltılır

        if (currentHealth < 0)
        {
            currentHealth = 0; // Sağlık negatif olmasın
        }

        // Sağlık UI'sini güncelleme (isteğe bağlı)
        UpdateHealthUI();
    }

    // Sağlık metnini güncelleme fonksiyonu (UI ile ilgili)
    void UpdateHealthUI()
    {
        // Burada sağlık UI'sini güncellemeyi sağlayabilirsiniz.
        // Örneğin, bir TextMeshPro veya UI Slider kullanabilirsiniz.
        Debug.Log("Health: " + currentHealth);
    }
}
