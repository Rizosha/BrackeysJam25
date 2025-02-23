using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sounds //Created sound class to label different sounds
    {
        public string Name;
        public AudioClip audioClip;
        public bool isLooping;
        [Range(0f, 1f)] public float volume;
        [Range(0f, 1f)] public float pitch;
    }

    public static AudioManager instance;

    public Sounds[] musicSounds, sfxSounds; //Creating two types of sounds

    private void Awake()
    {
        if(instance == null)
        instance = this;
    }

    public void PlayMusic(AudioSource source, string name)
    {
        Sounds[] sounds = musicSounds;

        if (source != null)
        {
            foreach (var sound in sounds)
            {
                if (sound.Name != name || sound.Name == null)
                    Debug.Log("No sound named " + name + " was found");

                if (sound.Name == name)
                {
                    source.clip = sound.audioClip;
                    source.volume = sound.volume;
                    source.pitch = sound.pitch;
                    source.loop = sound.isLooping;
                    source.Play();
                }
            }
        }
    }

    public void PlaySFX(AudioSource source, string name)
    {
        Sounds[] sounds = sfxSounds;

        if (source != null)
        {
            foreach (var sound in sounds)
            {
                if (sound.Name != name || sound.Name == null)
                    Debug.Log("No sound named " + name + " was found");
                    

                if (sound.Name == name)
                {
                    source.clip = sound.audioClip;
                    source.volume = sound.volume;
                    source.pitch = sound.pitch;
                    source.loop = sound.isLooping;
                    source.Play();

                }
            }
        }
    } 
    

    public void StopMusic(AudioSource source, string name)
    {
        Sounds[] sounds = musicSounds;

        if (source != null)
        {
            foreach (var sound in sounds)
            {
                if (sound.Name != name || sound.Name == null)
                    Debug.Log("No sound named " + name + " was found");

                if (sound.Name == name)
                {
                    source.clip = sound.audioClip;
                    source.Stop();
                }
            }
        } 
    }

    public void StopSFX(AudioSource source, string name)
    {
        Sounds[] sounds = sfxSounds;

        if (source != null)
        {
            foreach (var sound in sounds)
            {
                if (sound.Name != name || sound.Name == null)
                    Debug.Log("No sound named " + name + " was found");

                if (sound.Name == name)
                {
                    source.clip = sound.audioClip;
                    source.Stop();
                }
            }
        }
       
    }


    public void StopAllMusic()
    {
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        Sounds[] sounds = musicSounds;

        if (sources != null)
        {
            foreach (var source in sources)
            {
                foreach (var sound in sounds)
                {
                   source.clip = sound.audioClip;
                   source.Stop();
                }
            }
        }
    }

    public void StopAllSFX()
    {
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        Sounds[] sounds = sfxSounds;

        if (sources != null)
        {
            foreach (var source in sources)
            {
                foreach (var sound in sounds)
                {
                    source.clip = sound.audioClip;
                    source.Stop();
                }
            }
        }
    }
}
