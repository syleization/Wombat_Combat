using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SoundType
    {
        None,
        Music,
        Effect
    }

    [System.Serializable]
    public class ManagedSound
    {
        public string name;

        public SoundType type;

        public AudioClip clip;

        [Range(0, 1)]
        public float volume;

        ManagedSound()
        {
            name = "not set";
            type = SoundType.None;
            clip = null;
            volume = 1;
        }
    }

    private SoundManager() { }

    private static SoundManager TheInsatance;

    public static SoundManager Instance
    {
        get
        {
            if (TheInsatance == null)
            {
                TheInsatance = FindObjectOfType<SoundManager>();
                TheInsatance.Initialize();
            }

            return TheInsatance;
        }
    }

    public Dictionary<string, ManagedSound> SoundDictionary;

    [SerializeField]
    private List<ManagedSound> SoundList;

    [Range(0, 1)]
    public float MasterVolume = 1;

    [Range(0, 1)]
    public float MusicVolume = 1;

    [Range(0, 1)]
    public float EffectVolume = 1;

    private Transform camTransform;
    private AudioSource musicSource;

    private void Initialize()
    {
        camTransform = Camera.main.transform;
        SoundDictionary = new Dictionary<string, ManagedSound>();

        foreach (ManagedSound item in SoundList)
        {
            if (SoundDictionary.ContainsKey(item.name))
            {
                Debug.LogWarning("[SoundManager] - Contains 2 sounds named " + item.name);
            }
            else
            {
                SoundDictionary.Add(item.name, item);
            }
        }

        musicSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string key)
    {
        ManagedSound sound = null;

        if (SoundDictionary.TryGetValue(key, out sound))
        {
            float volume;

            switch (sound.type)
            {
                case SoundType.Music:
                    volume = MusicVolume;
                    break;
                case SoundType.Effect:
                    volume = EffectVolume;
                    break;
                default:
                    return;
            }

            if (volume > MasterVolume)
            {
                volume = MasterVolume;
            }

            volume = volume * sound.volume;

            switch (sound.type)
            {
                case SoundType.Music:
                    musicSource.Stop();
                    musicSource.clip = sound.clip;
                    musicSource.volume = volume;
                    musicSource.Play();
                    break;
                case SoundType.Effect:
                    AudioSource.PlayClipAtPoint(sound.clip, camTransform.position, volume);
                    break;
                default:
                    return;
            }

        }
    }
}
