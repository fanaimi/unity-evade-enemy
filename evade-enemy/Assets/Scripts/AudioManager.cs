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

    /// <summary>
    /// use in update / fixedUpdate
    /// </summary>
    /// <param name="name"></param>
    public void LerpVolumeToMax(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            // Debug.Log("Lerping up " + name);
            s.source.volume = Mathf.MoveTowards(s.source.volume, 1.0f, Time.deltaTime * 1.5f);
            // s.source.pitch = Mathf.MoveTowards(s.source.pitch, 3f, Time.deltaTime * 1.5f);
        }
        else
        {
            Debug.LogWarning($"could not find sound: {name}!");
        }
    }
    
    
    /// <summary>
    /// use in update / fixedUpdate
    /// </summary>
    /// <param name="name"></param>
    public void LerpVolumeToMin(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            Debug.Log("Lerping down " + name);
            s.source.volume = Mathf.MoveTowards(s.source.volume, 0.2f, Time.deltaTime * 2.0f);
            if (s.source.volume == 0)
            {
                Stop(s.source.name);
            }
        }
        else
        {
            Debug.LogWarning($"could not find sound: {name}!");
        }
    }

}
