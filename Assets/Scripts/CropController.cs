using UnityEngine;

public class CropController : MonoBehaviour
{
    [Header("Crop Settings")]
    public string cropName = "Carrot";
    public bool isPlanted = true;
    public float respawnTime = 10f;
    
    [Header("Audio")]
    public AudioClip collectSound;
    
    private SpriteRenderer spriteRenderer;
    private Collider2D cropCollider;
    private Vector3 originalPosition;
    private bool isCollected = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cropCollider = GetComponent<Collider2D>();
        originalPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPlanted && !isCollected)
        {
            Collect();
        }
    }

    public void Harvest()
    {
        if (!isPlanted || isCollected) return;
        
        isPlanted = false;
        Debug.Log($"{cropName} harvested (ready to collect)!");
        
        // Visual feedback
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 0.7f;
            spriteRenderer.color = color;
        }
    }

    void Collect()
    {
        isCollected = true;
        
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        
        Debug.Log($"{cropName} collected!");
        
        // Update UI
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.AddCrop(cropName);
        }
        
        // Hide crop
        spriteRenderer.enabled = false;
        cropCollider.enabled = false;
        
        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        isPlanted = true;
        isCollected = false;
        spriteRenderer.enabled = true;
        cropCollider.enabled = true;
        
        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
        
        Debug.Log($"{cropName} respawned!");
    }
}