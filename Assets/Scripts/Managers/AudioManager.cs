using UnityEngine;
using System.Collections.Generic;
using System.Linq; // For using Dictionary

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [Tooltip("AudioSource for background music.")]
    public AudioSource musicSource; 

    [Tooltip("AudioSource for sound effects. It's good to have a separate one.")]
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    [Tooltip("Drag all your background music AudioClips here.")]
    public AudioClip[] musicClips; 

    [Tooltip("Drag all your sound effect AudioClips here.")]
    public AudioClip[] sfxClips;
    
    private Dictionary<string, AudioClip> sfxDictionary;
    private Dictionary<string, AudioClip> musicDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
            return;
        }

        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in sfxClips)
        {
            if (!sfxDictionary.ContainsKey(clip.name)) 
            {
                sfxDictionary.Add(clip.name, clip);
            }
            else
            {
                Debug.LogWarning($"Duplicate SFX clip name found: {clip.name}. Only the first will be added.");
            }
        }

        musicDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in musicClips)
        {
            if (!musicDictionary.ContainsKey(clip.name))
            {
                musicDictionary.Add(clip.name, clip);
            }
            else
            {
                Debug.LogWarning($"Duplicate Music clip name found: {clip.name}. Only the first will be added.");
            }
        }

        musicSource.volume = .5f;
        sfxSource.volume = .5f;
        
        PlayMusic(musicDictionary.FirstOrDefault().Key);
    }
    
    public void PlayMusic(string clipName, bool loop = true)
    {
        if (musicDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music clip '{clipName}' not found!");
        }
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
    }
    
    public void PlaySFX(string clipName, float volumeScale = 1f)
    {
        if (sfxDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip, volumeScale);
        }
        else
        {
            Debug.LogWarning($"SFX clip '{clipName}' not found!");
        }
    }
    
    
    public void SetMusicVolume(float volume)
    {
        if (volume == 0)
        {
            musicSource.volume = 0;
            return;
        }
        float volumeInDb = Mathf.Log10(volume) * 20;

        musicSource.volume = Mathf.Pow(10.0f, volumeInDb / 20.0f);

    }
    
    public void SetSFXVolume(float volume)
    {
        if (volume == 0)
        {
            sfxSource.volume = 0;
            return;
        }

        float volumeInDb = Mathf.Log10(volume) * 20;
        sfxSource.volume = Mathf.Pow(10.0f, volumeInDb / 20.0f);
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }

    public float GetSFXVolume()
    {
        return sfxSource.volume;
    }

    public void MuteSFX(bool mute)
    {
        sfxSource.mute = mute;
    }
}