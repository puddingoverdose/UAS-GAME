using UnityEngine;
using TMPro;

public class SimpleUIManager : MonoBehaviour
{
    public static SimpleUIManager instance;
    public TextMeshProUGUI cropCounter;
    private int carrotCount = 0;
    
    void Awake()
    {
        instance = this;
    }
    
    public void AddCarrot()
    {
        carrotCount++;
        cropCounter.text = "Carrots: " + carrotCount;
        Debug.Log("Total carrots: " + carrotCount);
    }
}