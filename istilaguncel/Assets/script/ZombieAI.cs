using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player; // Oyuncunun Transform bileşeni
    public float viewDistance = 10f; // Görüş mesafesi
    public float attackDistance = 2f; // Saldırı mesafesi
    private NavMeshAgent agent;
    private Animator animator; // Animasyon kontrolü

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Animator bileşeni
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Oyuncuya yaklaşma
            if (distanceToPlayer <= viewDistance)
            {
                agent.SetDestination(player.position); // Zombiyi oyuncuya yönlendir

                if (distanceToPlayer <= attackDistance)
                {
                    // Saldırı animasyonu veya başka bir davranış başlatılabilir
                    animator.SetTrigger("Attack");
                }
                else
                {
                    // Koşma animasyonu (veya yürüme)
                    animator.SetBool("IsRunning", true);
                }
            }
            else
            {
                // Görüş mesafesinin dışındaysa hareket etmeyi durdur
                agent.SetDestination(transform.position);
                animator.SetBool("IsRunning", false); // Duraklama animasyonu
            }
        }
    }
}
