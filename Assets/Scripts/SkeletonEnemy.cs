using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHits = 3;
    private int currentHits = 0;
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 0.8f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;
    
    [Header("State")]
    private bool isAlive = true;
    private bool isDead = false;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    [Header("Audio")]
    public AudioClip attackSound;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (!isAlive || isDead || player == null) return;
        
        float distance = Vector2.Distance(transform.position, player.position);
        
        // Detection and chase
        if (distance <= detectionRange && distance > attackRange)
        {
            ChasePlayer();
        }
        else if (distance <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            animator.SetFloat("Speed", 0);
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        
        animator.SetFloat("Speed", 1);
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        
        if (direction.x != 0)
            spriteRenderer.flipX = direction.x < 0;
    }

    void AttackPlayer()
    {
        lastAttackTime = Time.time;
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Attack");
        
        if (attackSound != null)
            audioSource.PlayOneShot(attackSound);
        
        Debug.Log("Skeleton attacks player!");
        
        // Damage player
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (isDead) return;
        
        currentHits++;
        animator.SetTrigger("Hurt");
        
        if (hurtSound != null)
            audioSource.PlayOneShot(hurtSound);
        
        // Recoil
        Vector2 recoilDir = (transform.position - player.position).normalized;
        rb.AddForce(recoilDir * 3f, ForceMode2D.Impulse);
        
        Debug.Log($"Skeleton hit {currentHits}/{maxHits}");
        
        if (currentHits >= maxHits)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        isAlive = false;
        animator.SetTrigger("Death");
        
        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);
        
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        
        Debug.Log("Skeleton died! Respawning in 10 seconds...");
        
        Invoke("Respawn", 10f);
    }

    void Respawn()
    {
        currentHits = 0;
        isAlive = true;
        isDead = false;
        GetComponent<Collider2D>().enabled = true;
        animator.SetTrigger("Respawn");
        
        Debug.Log("Skeleton respawned!");
    }
}