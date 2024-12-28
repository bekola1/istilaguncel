using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int maxHealth = 500;  // Sağlık değeri artırıldı
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Zombi hasar alır
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;  // Hasar alındığında sağlık azalır
        Debug.Log("Zombi hasar aldı, mevcut sağlık: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();  // Sağlık sıfıra eşitse ölür
        }
    }

    // Zombi öldü
    void Die()
    {
        Debug.Log("Zombi öldü!");
        Destroy(gameObject);  // Zombiyi yok et
    }

    // Çarpışmayı algıla
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))  // Mermilerle çarpıştığında
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);  // Mermi hasarını al
                Destroy(collision.gameObject);  // Mermiyi yok et
            }
        }
    }
}
