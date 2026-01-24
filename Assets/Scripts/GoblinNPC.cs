using UnityEngine;

public class GoblinNPC : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1f;
    public float wanderRadius = 2f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveTimer;
    public float moveInterval = 4f;
    
    [Header("Audio")]
    public AudioClip goblinSound;
    public float soundInterval = 6f;
    private float soundTimer;
    private AudioSource audioSource;
    
    [Header("Emotes")]
    public GameObject heartEmote;
    public GameObject heartbreakEmote;
    private GameObject currentEmote;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        soundTimer = soundInterval;
    }

    void Update()
    {
        // Wander like animals
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveInterval)
        {
            moveTimer = 0;
            Vector2 randomDir = Random.insideUnitCircle * wanderRadius;
            targetPosition = startPosition + new Vector3(randomDir.x, randomDir.y, 0);
        }
        
        // Move
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector2 direction = (targetPosition - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
            
            if (direction.x != 0)
                spriteRenderer.flipX = direction.x < 0;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        // Sound
        soundTimer -= Time.deltaTime;
        if (soundTimer <= 0)
        {
            if (goblinSound != null && audioSource != null)
                audioSource.PlayOneShot(goblinSound);
            soundTimer = soundInterval;
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
        }
        
        Invoke("HideEmote", 2f);
    }

    void HideEmote()
    {
        if (currentEmote != null)
            Destroy(currentEmote);
    }
}