using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    
    [Header("Animation")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    [Header("Attack")]
    public Transform attackPoint;
    public float attackRange = 0.6f;
    public LayerMask enemyLayers;
    public float attackCooldown = 0.4f;
    private float lastAttackTime;
    
    [Header("Digging")]
    public float digRange = 1f;
    public float digCooldown = 0.5f;
    private float lastDigTime;
    
    [Header("Audio")]
    public AudioClip attackSound;
    public AudioClip digSound;
    public AudioClip hurtSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        // Movement Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        // Update animator
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        
        // Flip sprite
        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;
        
        // Attack Action (Space key)
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
        
        // Dig Action (E key)
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastDigTime + digCooldown)
        {
            Dig();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
    
    void Attack()
    {
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");
        
        // Play sound
        if (attackSound != null)
            audioSource.PlayOneShot(attackSound);
        
        Debug.Log("Player Attack!");
        
        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            SkeletonEnemy skeleton = enemy.GetComponent<SkeletonEnemy>();
            if (skeleton != null)
            {
                skeleton.TakeDamage();
                Debug.Log("Hit skeleton!");
            }
            
            AnimalNPC animal = enemy.GetComponent<AnimalNPC>();
            if (animal != null)
            {
                animal.ShowHeartbreak();
                Debug.Log("Hit animal - heartbreak!");
            }
            
            GoblinNPC goblin = enemy.GetComponent<GoblinNPC>();
            if (goblin != null)
            {
                goblin.ShowHeartbreak();
                Debug.Log("Hit goblin - heartbreak!");
            }
        }
    }
    
    void Dig()
    {
        lastDigTime = Time.time;
        animator.SetTrigger("Dig");
        
        // Play sound
        if (digSound != null)
            audioSource.PlayOneShot(digSound);
        
        Debug.Log("Player Digging!");
        
        // Check for crops nearby
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, digRange);
        
        foreach (Collider2D obj in nearbyObjects)
        {
            CropController crop = obj.GetComponent<CropController>();
            if (crop != null && crop.isPlanted)
            {
                crop.Harvest();
                Debug.Log("Harvested crop!");
            }
        }
    }
    
    public void TakeDamage()
    {
        animator.SetTrigger("Hurt");
        
        if (hurtSound != null)
            audioSource.PlayOneShot(hurtSound);
        
        // Apply recoil
        Vector2 recoilDirection = -movement.normalized;
        if (recoilDirection == Vector2.zero)
            recoilDirection = Vector2.down;
        rb.AddForce(recoilDirection * 5f, ForceMode2D.Impulse);
        
        Debug.Log("Player hurt!");
    }
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, digRange);
    }
}