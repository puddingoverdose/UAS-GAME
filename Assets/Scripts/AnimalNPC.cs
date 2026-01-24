using UnityEngine;

public class AnimalNPC : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1f;
    public float wanderRadius = 3f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveTimer;
    public float moveInterval = 3f;
    
    [Header("Audio")]
    public AudioClip animalSound;
    public float soundInterval = 5f;
    private float soundTimer;
    private AudioSource audioSource;
    
    [Header("Emotes")]
    public GameObject heartEmote;
    public GameObject heartbreakEmote;
    private GameObject currentEmote;
    
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        
        soundTimer = soundInterval;
    }

    void Update()
    {
        // Wander
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveInterval)
        {
            moveTimer = 0;
            SetNewTarget();
        }
        
        MoveToTarget();
        
        // Play sound periodically
        soundTimer -= Time.deltaTime;
        if (soundTimer <= 0)
        {
            PlaySound();
            soundTimer = soundInterval;
        }
    }

    void SetNewTarget()
    {
        Vector2 randomDir = Random.insideUnitCircle * wanderRadius;
        targetPosition = startPosition + new Vector3(randomDir.x, randomDir.y, 0);
    }

    void MoveToTarget()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector2 direction = (targetPosition - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
            
            if (animator != null)
            {
                animator.SetFloat("Speed", 1);
                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
            }
            
            if (direction.x != 0 && spriteRenderer != null)
                spriteRenderer.flipX = direction.x < 0;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null)
                animator.SetFloat("Speed", 0);
        }
    }

    void PlaySound()
    {
        if (animalSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(animalSound);
            Debug.Log($"{gameObject.name} makes sound!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowHeart();
        }
    }

    public void ShowHeart()
    {
        if (currentEmote != null)
            Destroy(currentEmote);
        
        if (heartEmote != null)
        {
            currentEmote = Instantiate(heartEmote, transform.position + Vector3.up * 1f, Quaternion.identity);
            currentEmote.transform.SetParent(transform);
            Debug.Log($"{gameObject.name} shows heart!");
        }
        
        Invoke("HideEmote", 2f);
    }

    public void ShowHeartbreak()
    {
        if (currentEmote != null)
            Destroy(currentEmote);
        
        if (heartbreakEmote != null)
        {
            currentEmote = Instantiate(heartbreakEmote, transform.position + Vector3.up * 1f, Quaternion.identity);
            currentEmote.transform.SetParent(transform);
            Debug.Log($"{gameObject.name} shows heartbreak!");
        }
        
        Invoke("HideEmote", 2f);
    }

    void HideEmote()
    {
        if (currentEmote != null)
            Destroy(currentEmote);
    }
}