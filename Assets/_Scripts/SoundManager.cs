using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public enum SoundType
{
    EXPLOSION_1 = 1,
    EXPLOSION_2 = 2,
    EXPLOSION_3 = 3,
    BULLET_HIT = 4,
    PLAYER_SHOOT = 5
}

public class SoundManager : MonoBehaviour {

    private static SoundManager _instance = null;
    public static SoundManager Instance
    {
        get
        {
            if(_instance == null)
            {
                 Debug.LogError("No SoundManager was found");
            }

            return _instance;
        }
    }

    [SerializeField]
    AudioMixer Mixer;

    [SerializeField]
    AudioMixerGroup SfxGroup;
    
    private List<AudioSource> _sfxSources;
    private Dictionary<SoundType, AudioClip> _audioClipMap = new Dictionary<SoundType, AudioClip>();
    
    void Start()
    {
        Debug.Log("Soundmanager instance : " + (_instance != null));
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(this.gameObject);
            return;
        }
    }

    public void PlayClip(SoundType type)
    {
        AudioClip clip = null;
        if(_audioClipMap.ContainsKey(type))
        {
            clip = _audioClipMap[type];
        }
        else
        {
            clip = LoadAudioClip(type);
        }
        
        if(clip == null)
        {
            return;
        }

        //AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        GameObject go = new GameObject("SFX Source");
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.outputAudioMixerGroup = SfxGroup;
        source.Play();
        Destroy(go, clip.length);
    }

    public void ToggleSounds(bool on)
    {
        float volume = on ? 0 : -80f;

        Mixer.SetFloat("MasterVolume", volume);
    }

    public bool SoundsOn()
    {
        float soundVolume;
        Mixer.GetFloat("MasterVolume", out soundVolume);
        return soundVolume == 0f;
    }

    private AudioClip LoadAudioClip(SoundType type)
    {
        AudioClip clip = null;

        string path = "Audio/";

        switch(type)
        {
            case SoundType.EXPLOSION_1:
                path += "Explosion1";
                break;
            case SoundType.EXPLOSION_2:
                path += "Explosion2";
                break;
            case SoundType.EXPLOSION_3:
                path += "Blast";
                break;
            case SoundType.BULLET_HIT:
                path += "hitSound";
                break;
            case SoundType.PLAYER_SHOOT:
                path += "PickUp";
                break;
        }

        clip = Resources.Load<AudioClip>(path);
        if(clip == null)
        {
            Debug.LogError("No clip found for " + type.ToString());
        }
        return clip;
    }
}
