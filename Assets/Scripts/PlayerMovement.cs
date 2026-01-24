using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    [Header("Attack")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Attack input - NO LONGER CHECKS isAttacking
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        
        // Movement - REMOVED the !isAttacking check
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        animator.SetFloat("Speed", movement.sqrMagnitude);
        
        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;
    }

    void FixedUpdate()
    {
        // REMOVED the !isAttacking check - always move
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
    
    void Attack()
    {
        // Remove the isAttacking check if you want to attack spam
        animator.SetTrigger("Attack");
        Debug.Log("Player attacked while moving!");
        
        // Optional: Add attack cooldown instead
        // StartCoroutine(AttackCooldown());
    }
    
    // Optional: Add cooldown to prevent spam
    /*
    private bool canAttack = true;
    public float attackCooldown = 0.5f;
    
    void Attack()
    {
        if (!canAttack) return;
        
        canAttack = false;
        animator.SetTrigger("Attack");
        Debug.Log("Player attacked!");
        
        Invoke("ResetAttackCooldown", attackCooldown);
    }
    
    void ResetAttackCooldown()
    {
        canAttack = true;
    }
    */
}