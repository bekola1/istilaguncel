using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;  // Mermi hasarı düşürüldü

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))  // Zombi ile çarpışma
        {
            ZombieHealth zombieHealth = collision.gameObject.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(damage);  // Zombi hasar alır
            }
        }

        Destroy(gameObject);  // Mermiyi yok et
    }
}
