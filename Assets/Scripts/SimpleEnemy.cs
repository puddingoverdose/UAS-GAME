using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 2f;
    public float detectionRange = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
            
        Debug.Log("Enemy spawned!");
    }

    void Update()
    {
        if (player == null) return;
        
        float distance = Vector2.Distance(transform.position, player.position);
        
        if (distance < detectionRange)
        {
            // Chase player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * chaseSpeed;
            
            // Flip sprite
            if (direction.x < 0)
                spriteRenderer.flipX = true;
            else if (direction.x > 0)
                spriteRenderer.flipX = false;
                
            Debug.Log("Chasing player! Distance: " + distance);
        }
        else
        {
            // STOP COMPLETELY when not chasing
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f; // Stop any rotation too
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}