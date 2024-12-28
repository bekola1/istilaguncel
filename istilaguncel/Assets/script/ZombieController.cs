using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  // NavMeshAgent için gerekli

public class ZombieController : MonoBehaviour
{
    public int health = 50; // Zombinin başlangıç sağlığı
    public int damage = 10; // Zombinin vereceği hasar
    public float attackRange = 2f; // Saldırı mesafesi
    public float attackRate = 1f; // Saldırı hızı
    private float nextAttackTime = 0f; // Bir sonraki saldırı zamanı

    private Transform player; // Oyuncu Transformu
    private PlayerHealth playerHealth; // Oyuncunun sağlık sistemi

    private NavMeshAgent navMeshAgent; // NavMeshAgent bileşeni

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Oyuncu nesnesini bul
        playerHealth = player.GetComponent<PlayerHealth>(); // Oyuncunun sağlık sistemini al
        navMeshAgent = GetComponent<NavMeshAgent>(); // NavMeshAgent bileşenini al

        // NavMeshAgent ayarlarını yapalım
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = 3.5f; // Zombinin hareket hızı
            navMeshAgent.angularSpeed = 120f; // Yön değiştirme hızı
            navMeshAgent.acceleration = 8f; // Hızlanma değeri
        }
        else
        {
            Debug.LogError("NavMeshAgent bileşeni bulunamadı!");
        }
    }

    void Update()
    {
        // Zombi ile oyuncu arasındaki mesafeyi hesapla
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (Time.time >= nextAttackTime)
            {
                AttackPlayer(); // Oyuncuya saldır
                nextAttackTime = Time.time + 1f / attackRate; // Saldırı hızını ayarla
            }
        }
        else
        {
            // Zombi oyuncuya yaklaşmaya devam et
            MoveTowardsPlayer();
        }
    }

    // Oyuncuya saldırma fonksiyonu
    private void AttackPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage); // Oyuncuya hasar ver
            Debug.Log("Zombi oyuncuya saldırdı! Hasar verdi: " + damage);
        }
    }

    // Zombinin oyuncuya doğru hareket etmesi
    private void MoveTowardsPlayer()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.SetDestination(player.position); // Hedef olarak oyuncu pozisyonunu belirle
            transform.LookAt(player); // Zombiyi oyuncuya doğru döndür
        }
    }

    // Zombi hasar aldığında bu fonksiyon çağrılır
    public void TakeDamage(int damage)
    {
        health -= damage; // Zombiye hasar ver
        Debug.Log("Zombi hasar aldı, kalan sağlık: " + health);

        if (health <= 0)
        {
            Die(); // Zombi öldüğünde ölüm fonksiyonunu çağır
        }
    }

    // Zombi öldüğünde çağrılacak fonksiyon
    private void Die()
    {
        Debug.Log("Zombi öldü");
        Destroy(gameObject); // Zombiyi yok et
    }
}
