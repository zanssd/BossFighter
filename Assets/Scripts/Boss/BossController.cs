using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public BossAttackPattern[] attackPatterns;
    public float moveSpeed = 3f;
    public Transform[] hoverPoints;
    public int maxHits = 3;
    public int currentHits = 0;
    private Transform currentTargetHoverPoint;
    public float hoverMoveSpeed = 2f;
    public GameObject shadowPrefab;
    public float eagleStrikeHeight = 20f;
    public float slamSpeed = 30f;
    public Transform flyingAway;
    public GameObject rocketPrefab;
    public Transform rocketSpawnPoint;
    [SerializeField]
    private Animator animator;
    private bool isAttacking = false;
    private bool eagleStrikeActive = false;


    private void Start()
    {
        StartCoroutine(AttackCycle());
    }

    void Update()
    {
        HoverMovement();
    }

    void HoverMovement()
    {
        if (isAttacking) return;
        if (currentTargetHoverPoint == null || Vector3.Distance(transform.position, currentTargetHoverPoint.position) < 0.5f)
        {
            currentTargetHoverPoint = hoverPoints[Random.Range(0, hoverPoints.Length)];
        }

        transform.position = Vector3.MoveTowards(transform.position, currentTargetHoverPoint.position, hoverMoveSpeed * Time.deltaTime);
    }
    IEnumerator AttackCycle()
    {
        while (currentHits < maxHits)
        {
            foreach (var pattern in attackPatterns)
            {
                yield return new WaitForSeconds(pattern.attackDelay);
                yield return StartCoroutine(PerformAttack(pattern));
            }
        }

        Defeated();
    }

    IEnumerator PerformAttack(BossAttackPattern pattern)
    {
        switch (pattern.attackType)
        {
            case BossAttackType.FireFlame:
                Vector3 target = GetRandomPlayerPosition();
                if (target != null)
                {
                    yield return StartCoroutine(ShootFlameAtPlayerAndWait(pattern.visualEffectPrefab,target));
                }
                break;

            case BossAttackType.EagleStrike:
                yield return StartCoroutine(EagleStrike(pattern)); 
                break;

            case BossAttackType.RocketLaunch:
                yield return StartCoroutine(RocketLaunchRoutine(pattern.visualEffectPrefab));
                break;
        }

        if (pattern.soundEffect)
            AudioSource.PlayClipAtPoint(pattern.soundEffect, transform.position);
    }

    IEnumerator EagleStrike(BossAttackPattern pattern)
    {
        eagleStrikeActive = true;
        if (animator != null)
        {
            animator.SetBool("isEagleStrike", true);
        }
        Vector3 targetPosition = GetRandomPlayerPosition();

        while (Vector3.Distance(transform.position, flyingAway.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, flyingAway.position, slamSpeed * Time.deltaTime);
            yield return null;
        }
        GameObject shadow = Instantiate(shadowPrefab, targetPosition, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        transform.position = targetPosition + Vector3.up * eagleStrikeHeight;
        gameObject.SetActive(true);

        while (transform.position.y > targetPosition.y + 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, slamSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(shadow);

        Transform closestHover = GetClosestHoverPoint();
        yield return StartCoroutine(ReturnToHover(closestHover.position));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!eagleStrikeActive) return;

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
            }
        }
    }


    IEnumerator ReturnToHover(Vector3 targetPos)
    {
        eagleStrikeActive = false;
        if (animator != null)
        {
            animator.SetBool("isEagleStrike", false);
        }
        float duration = 1f;
        float time = 0f;
        Vector3 startPos = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }

    Transform GetClosestHoverPoint()
    {
        Transform closest = hoverPoints[0];
        float minDist = Vector3.Distance(transform.position, closest.position);

        foreach (var point in hoverPoints)
        {
            float dist = Vector3.Distance(transform.position, point.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = point;
            }
        }
        return closest;
    }

    IEnumerator RocketLaunchRoutine(GameObject objToSpawn)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) yield break;

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 3; i++)
        {
            GameObject rocket = Instantiate(objToSpawn, rocketSpawnPoint.position, Quaternion.identity);
            RocketPrefab rp = rocket.GetComponent<RocketPrefab>();

            Vector3 target = GetRandomPlayerPosition();

            rp.SetTarget(target);

            if (i == 2)
            {
                rp.canBePickedUp = true;
                rocket.tag = "Rocket"; 
            }

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(2f);
    }

    IEnumerator ShootFlameAtPlayerAndWait(GameObject obj,Vector3 player)
    {
        isAttacking = true;

        Vector3 direction = player - transform.position;
        float distance = direction.magnitude;
        Vector3 midPoint = transform.position + direction / 2f;
        Quaternion rotation = Quaternion.LookRotation(direction);

        yield return new WaitForSeconds(1f);
        GameObject beam = Instantiate(obj, midPoint, rotation);
        beam.transform.localScale = new Vector3(0.5f, 0.5f, distance);

        BoxCollider col = beam.GetComponent<BoxCollider>();
        if (col != null)
        {
            col.size = new Vector3(1f, 1f, distance);
            col.center = new Vector3(0f, 0f, distance / 2f);
        }

        float flameDuration = 2f;
        yield return new WaitForSeconds(flameDuration);
        Destroy(beam);
        isAttacking = false;
    }




    public void TakeHitFromPlayer(int fromPlayer)
    {
        currentHits++;
        Debug.Log("Take hit from " + fromPlayer);

        if (fromPlayer == 1 || fromPlayer == 2)
            SaveManager.AddPlayerScore(fromPlayer, 100);
        if (currentHits >= maxHits)
        {
            Defeated();
        }
    }

    void Defeated()
    {
        GameManager.Instance.BossDefeated();
    }

    Vector3 GetRandomPlayerPosition()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return transform.position;

        GameObject target = players[Random.Range(0, players.Length)];
        return new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
    }
}
