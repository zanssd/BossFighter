using UnityEngine;

public class RocketPrefab : MonoBehaviour
{
    public RocketConfig config;
    public bool canBePickedUp = false;
    public bool isHold = false;
    private Vector3 target;
    private Transform targetBoss;
    private bool isReturnToBoss;
    public int fromPlayer = -1;


    private void Update()
    {
        if (target == null || config == null || isHold) return;

        if(isReturnToBoss)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetBoss.position, config.speed * Time.deltaTime);
            transform.LookAt(target);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target, config.speed * Time.deltaTime);
            transform.LookAt(target);
        }
       
    }

    public void SetTarget(Vector3 t)
    {
        target = t;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canBePickedUp && other.CompareTag("Player"))
        {
            return;
        }

        if (!canBePickedUp && other.CompareTag("Ground"))
        {
            ExplodeAndDestroy();
            return;
        }

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
                ExplodeAndDestroy();
            }
        }

        if (other.CompareTag("Obstacle"))
        {
            if (config.explodesOnImpact)
            {
                ExplodeAndDestroy();
            }
        }

        if (isReturnToBoss && other.CompareTag("Boss"))
        {
            BossController boss = other.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeHitFromPlayer(fromPlayer);
            }

            ExplodeAndDestroy();
            return;
        }
    }

    public void ReturnToBoss()
    {
        targetBoss = GameObject.FindWithTag("Boss").transform;
        canBePickedUp = false;
        isHold = false;
        isReturnToBoss = true;
    }

    void ExplodeAndDestroy()
    {
        if (config.explosionEffect != null)
        {
            Instantiate(config.explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
