using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class CropInventory
{
    public int carrots = 0;
    public int wheat = 0;
    public int tomatoes = 0;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("UI Panels")]
    public GameObject welcomePanel;
    public GameObject gameplayUI;
    
    [Header("Student Info")]
    public TextMeshProUGUI studentInfoText;
    public string studentName = "Ridho Mulia";
    public string studentID = "2702327103";
    
    [Header("Crop UI")]
    public TextMeshProUGUI carrotText;
    public TextMeshProUGUI wheatText;
    public TextMeshProUGUI tomatoText;
    
    [Header("Game State")]
    public bool isPaused = false;
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
        
        if (studentInfoText != null)
        {
            studentInfoText.text = $"Name: {studentName}\nStudent ID: {studentID}";
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void ShowWelcomeScreen()
    {
        if (welcomePanel != null)
        {
            welcomePanel.SetActive(true);
            if (gameplayUI != null)
                gameplayUI.SetActive(false);
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void StartGame()
    {
        if (welcomePanel != null)
        {
            welcomePanel.SetActive(false);
            if (gameplayUI != null)
                gameplayUI.SetActive(true);
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (welcomePanel != null)
            welcomePanel.SetActive(isPaused);
        if (gameplayUI != null)
            gameplayUI.SetActive(!isPaused);
        
        Time.timeScale = isPaused ? 0 : 1;
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
            case "tomato":
            case "tomatoes":
                inventory.tomatoes++;
                break;
        }
        
        UpdateCropUI();
        SaveCropData();
        Debug.Log($"{cropName} added! Total: {inventory.carrots} carrots, {inventory.wheat} wheat, {inventory.tomatoes} tomatoes");
    }

    void UpdateCropUI()
    {
        if (carrotText != null)
            carrotText.text = $"🥕 {inventory.carrots}";
        if (wheatText != null)
            wheatText.text = $"🌾 {inventory.wheat}";
        if (tomatoText != null)
            tomatoText.text = $"🍅 {inventory.tomatoes}";
    }

    void SaveCropData()
    {
        string json = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Saved to: {saveFilePath}");
    }

    void LoadCropData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            inventory = JsonUtility.FromJson<CropInventory>(json);
            Debug.Log($"Loaded from: {saveFilePath}");
        }
        else
        {
            inventory = new CropInventory();
            Debug.Log("No save file, starting fresh");
        }
    }
}