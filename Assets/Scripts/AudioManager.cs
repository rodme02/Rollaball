using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;       // AudioSource for background music
    public AudioSource sfxSource;         // AudioSource for sound effects

    public AudioClip mainMenuMusic;       // Main Menu background music
    public AudioClip gameSceneMusic;      // Game Scene background music
    public AudioClip buttonClickSound;    // Button click sound effect
    public AudioClip gameOverSound;       // Game Over sound effect
    public AudioClip pickupSound;         // Pickup sound effect
    public AudioClip jumpSound;           // Jump sound effect
    public AudioClip winSound;            // Win sound effect
    public AudioClip deathSound;          // Death sound effect

    private void Awake()
    {
        // Ensure there's only one instance of AudioManager (Singleton pattern)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }

    // Method to play background music
    public void PlayMusic(AudioClip musicClip)
    {   
        if (musicSource.clip == musicClip && musicSource.isPlaying)
        {
            return; 
        }

        musicSource.clip = musicClip;  // Assign the new clip
        musicSource.loop = true;       // Ensure music loops by default (can be changed based on the clip)
        musicSource.Play();
        
    }

    // Method to stop background music
    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();           // Stop the current music
            musicSource.clip = null;      // Reset the clip to avoid conflicts
            musicSource.loop = false;     // Reset looping to default
        }
    }


    // Method to play sound effects
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
}
