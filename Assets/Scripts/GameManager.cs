using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

[System.Serializable]
public class CropInventory
{
    public int carrots = 0;
    public int wheat = 0;
    public int cabbages = 0;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("UI Panels")]
    public GameObject welcomePanel;
    public GameObject gameplayUI;
    
    [Header("Student Info")]
    public string studentName = "PUT YOUR NAME HERE";
    public string studentID = "PUT YOUR ID HERE";
    
    [Header("Crop UI")]
    public TextMeshProUGUI carrotText;
    public TextMeshProUGUI wheatText;
    public TextMeshProUGUI cabbageText;
    
    private bool isPaused = false;
    private CropInventory inventory;
    private string saveFilePath;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        saveFilePath = Application.persistentDataPath + "/cropdata.json";
    }

    void Start()
    {
        LoadCropData();
        UpdateCropUI();
        ShowWelcomeScreen();
        
        Debug.Log("GameManager started!");
    }

    void Update()
    {
        // ESC key pauses
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void ShowWelcomeScreen()
    {
        if (welcomePanel != null)
            welcomePanel.SetActive(true);
        
        if (gameplayUI != null)
            gameplayUI.SetActive(false);
        
        Time.timeScale = 0; // Pause game
        isPaused = true;
        
        Debug.Log("Welcome screen shown - game paused");
    }

    public void StartGame()
    {
        Debug.Log("START BUTTON CLICKED!");
        
        if (welcomePanel != null)
            welcomePanel.SetActive(false);
        
        if (gameplayUI != null)
            gameplayUI.SetActive(true);
        
        Time.timeScale = 1; // Unpause game
        isPaused = false;
        
        Debug.Log("Game started - unpaused");
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            // Show pause menu
            if (welcomePanel != null)
                welcomePanel.SetActive(true);
            if (gameplayUI != null)
                gameplayUI.SetActive(false);
            Time.timeScale = 0;
            Debug.Log("Game paused");
        }
        else
        {
            // Resume game
            if (welcomePanel != null)
                welcomePanel.SetActive(false);
            if (gameplayUI != null)
                gameplayUI.SetActive(true);
            Time.timeScale = 1;
            Debug.Log("Game resumed");
        }
    }

    public void AddCrop(string cropName)
    {
        switch (cropName.ToLower())
        {
            case "carrot":
                inventory.carrots++;
                break;
            case "wheat":
                inventory.wheat++;
                break;
            case "cabbage":
            case "cabbages":
                inventory.cabbages++;
                break;
        }
        
        UpdateCropUI();
        SaveCropData();
        Debug.Log($"Crop added: {cropName}. Totals - Carrots: {inventory.carrots}, Wheat: {inventory.wheat}, Cabbages: {inventory.cabbages}");
    }

    void UpdateCropUI()
    {
        if (carrotText != null)
            carrotText.text = "Carrots: " + inventory.carrots;
        
        if (wheatText != null)
            wheatText.text = "Wheat: " + inventory.wheat;
        
        if (cabbageText != null)
            cabbageText.text = "Cabbages: " + inventory.cabbages;
    }

    void SaveCropData()
    {
        string json = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Crop data saved to: " + saveFilePath);
    }

    void LoadCropData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            inventory = JsonUtility.FromJson<CropInventory>(json);
            Debug.Log("Crop data loaded!");
        }
        else
        {
            inventory = new CropInventory();
            Debug.Log("No save file found, starting fresh");
        }
    }
}