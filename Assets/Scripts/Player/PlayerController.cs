using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerID = 0;
    public float moveSpeed = 5f;
    public PlayerInputConfig inputConfig;
    public int maxHealth = 3;
    private int currentHealth = 3;

    //private Rigidbody rb;
    private CharacterController controller;
    private Vector3 moveDirection;
    [SerializeField]
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        UIManager.Instance.RegisterPlayer(this, currentHealth);
        GameManager.Instance.RegisterPlayer(this);
    }

    void FixedUpdate()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(inputConfig.up)) vertical += 1f;
        if (Input.GetKey(inputConfig.down)) vertical -= 1f;
        if (Input.GetKey(inputConfig.left)) horizontal -= 1f;
        if (Input.GetKey(inputConfig.right)) horizontal += 1f;

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection;
        }

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (animator != null)
        {
            animator.SetBool("isMoving", moveDirection != Vector3.zero);
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
        UIManager.Instance.UpdateHealth(this, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.PlayerDied(this);
        gameObject.SetActive(false);
    }


}
