using System.Collections;
using UnityEngine;

public class RocketPickup : MonoBehaviour
{
    private GameObject heldRocket = null;
    [SerializeField] private Transform handToHold;
    [SerializeField] private PlayerController playerController;

    void Update()
    {
        if (heldRocket != null && Input.GetKeyDown(playerController.inputConfig.throwKey))
        {
            ThrowRocket();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (heldRocket == null && other.CompareTag("Rocket"))
        {
            RocketPrefab rocket = other.GetComponent<RocketPrefab>();
            if (rocket != null && rocket.canBePickedUp)
            {
                rocket.isHold = true;
                heldRocket = rocket.gameObject;
                heldRocket.transform.SetParent(handToHold);
                heldRocket.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    void ThrowRocket()
    {
        heldRocket.transform.SetParent(null);
        Rigidbody rb = heldRocket.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Collider rocketCol = heldRocket.GetComponent<Collider>();
        if (rocketCol != null)
        {
            rocketCol.enabled = false;
            StartCoroutine(ReenableCollider(rocketCol, 0.3f));
        }
        RocketPrefab rocket = heldRocket.GetComponent<RocketPrefab>();
        rocket.fromPlayer = (playerController.playerID == 1) ? 1 : 2; 
        if (rocket != null)
        {
            rocket.ReturnToBoss();
        }

        heldRocket = null;
    }

    IEnumerator ReenableCollider(Collider col, float delay)
    {
        yield return new WaitForSeconds(delay);
        col.enabled = true;
    }
}
