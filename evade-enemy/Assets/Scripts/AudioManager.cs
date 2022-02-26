using UnityEngine.Audio;
using UnityEngine;
using System;

// https://www.youtube.com/watch?v=6OT43pvUyfY&ab_channel=Brackeys

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }


        // we want audioManager to persist through different scenes
        DontDestroyOnLoad(gameObject);
        
        
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        
    }

    public void Play(string name)
    {
        // because of "using System" at the top
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning($"could not find sound: {name}!");
        }

    }
    
    
    public void PlayOnce(string name)
    {
        // because of "using System" at the top
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null && !s.source.isPlaying)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning($"could not find sound: {name}!");
        }

    }
    
    
    public void Stop(string name)
    {
        // because of "using System" at the top
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogWarning($"could not find sound: {name}!");
        }

    }
    
    public void Pause(string name)
    {
        // because of "using System" at the top
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Pause();
        }
        else
        {
            Debug.LogWarning($"could not find sound: {name}!");
        }

    }
}
