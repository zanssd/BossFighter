using UnityEngine;

public class FireFlame : MonoBehaviour
{
    public float duration = 2f;
    public float damageInterval = 0.5f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.TakeDamage();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CancelInvoke(nameof(DealDamage));
        }
    }

    void DealDamage()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerController>()?.TakeDamage();
            }
        }
    }
}
