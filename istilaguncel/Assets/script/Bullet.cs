using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;  // Merminin hasarı

    void OnCollisionEnter(Collision collision)
    {
        // Eğer mermi hâlâ var ve çarpışma gerçekleştiyse
        if (gameObject != null)
        {
            // Çarpışma nesnesine hasar uygulama
            if (collision.gameObject.CompareTag("Zombie"))
            {
                ZombieHealth zombieHealth = collision.gameObject.GetComponent<ZombieHealth>();
                if (zombieHealth != null)
                {
                    zombieHealth.TakeDamage(damage); // Zombi hasar alır
                }
            }

            // Mermiyi yok et
            Destroy(gameObject);
        }
    }
}
