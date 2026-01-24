using UnityEngine;

public class SimpleCollectible : MonoBehaviour
{
    public string itemName = "Carrot";
    
void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log(itemName + " collected!");
        
        // Add this null check!
        if (SimpleUIManager.instance != null)
        {
            SimpleUIManager.instance.AddCarrot();
        }
        
        Destroy(gameObject);
    }
}
}