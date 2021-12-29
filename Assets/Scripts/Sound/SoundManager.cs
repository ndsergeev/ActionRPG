using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton;
    public List<Sound> sounds = new List<Sound>();
    private static Dictionary<string, float> soundTimerDictionary;

    public static SoundManager instance
    {
        get
        {
            return Singleton;
        }
    }

    private void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Singleton = this;
        }

        soundTimerDictionary = new Dictionary<string, float>();

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.isLoop;
            sound.source.spatialBlend = sound.spatialBlend;

            if (sound.hasCooldown)
            {
                Debug.Log(sound.name);
                soundTimerDictionary[sound.name] = 0f;
            }
        }
    }

    private void Start()
    {
        // Add this part after having a theme song
        // Play('Theme');
    }
    public void Play(string name)
    {
        Sound sound = GetSound(name);

        if (!CanPlaySound(sound)) return;

        sound.source.Play();

        print("PLAYED SOUND!: " + name);
    }

    public void Stop(string name)
    {
        Sound sound = GetSound(name);

        sound.source.Stop();
    }

    private static bool CanPlaySound(Sound sound)
    {
        if (soundTimerDictionary.ContainsKey(sound.name))
        {
            float lastTimePlayed = soundTimerDictionary[sound.name];

            if (lastTimePlayed + sound.clip.length < Time.time)
            {
                soundTimerDictionary[sound.name] = Time.time;
                return true;
            }

            return false;
        }

        return true;
    }

    public Sound GetSound(string name)
    {
        Sound sound = sounds[0];

        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                sound = s;
                break;
            }
        }

        return sound;
    }

    public void StartFadeCoroutine(string name, float duration, float targetVolume)
    {
        StartCoroutine(StartFade(name, duration, targetVolume));
    }

    public IEnumerator StartFade(string name, float duration, float targetVolume)
    {
        print("start fade");

        Sound sound = SoundManager.Singleton.GetSound(name);

        AudioSource source = sound.source;
        float currentTime = 0;
        float start = source.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            source.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        if (currentTime >= duration)
        {
            if (targetVolume == 0)
            {
                SoundManager.Singleton.Stop(name);
            }
            

            print("finish fade");
        }
        yield break;
    }


}