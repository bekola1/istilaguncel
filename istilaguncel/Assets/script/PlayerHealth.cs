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
        currentHealth -= damage; // Hasar alındığında sağlık azalır

        if (currentHealth < 0)
        {
            currentHealth = 0; // Sağlık negatif olmasın
        }

        // Sağlık değiştiğinde herhangi bir işlem yapabilirsiniz
        // Örneğin UI güncellemesi burada yapılabilir.
        Debug.Log("Health: " + currentHealth);
    }

    // Oyuncu ölürse çağrılacak fonksiyon
    public void Die()
    {
        Debug.Log("Player has died!");
        // Oyuncu öldüğünde yapılacak işlemleri buraya ekleyebilirsiniz (Örneğin, game over ekranı).
        Destroy(gameObject); // Oyuncu nesnesini yok et
    }
}