using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    //sound assets contain the sound file, a name for it, as well as volume and pitch to be played at
    public SoundAsset[] SoundAssets;
    public static AudioManager instance;

    private void Awake()
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
        DontDestroyOnLoad(gameObject);
        foreach(SoundAsset s in SoundAssets)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }   
    }

    private void Start()
    {
        PlaySoundByName("BackgroundTrack");
    }
    public void PlaySoundByName(string name)
    {
        SoundAsset s = Array.Find(SoundAssets, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

}
