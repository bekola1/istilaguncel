using UnityEngine;
using UnityEngine.AI;  // NavMeshAgent için gerekli

public class ZombieAI : MonoBehaviour
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
        // Oyuncu nesnesini bul
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        // Oyuncunun sağlık sistemini al
        playerHealth = player.GetComponent<PlayerHealth>(); 
        // NavMeshAgent bileşenini al
        navMeshAgent = GetComponent<NavMeshAgent>(); 

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

        // Eğer zombi oyuncuya yakınsa
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
            // Oyuncuya yaklaşmaya devam et
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

            // Zombiyi oyuncuya doğru döndür
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0;  // Y eksenini sıfırlayarak sadece yatayda yönlendirme yap
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * 5f);
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
