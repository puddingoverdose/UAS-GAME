using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [Header("Music")]
    public AudioClip backgroundMusic;
    private AudioSource musicSource;
    
    [Header("Ambient")]
    public AudioClip[] ambientSounds;
    private AudioSource ambientSource;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Setup music
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = 0.3f;
        musicSource.Play();
        
        // Setup ambient
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.volume = 0.4f;
    }
}