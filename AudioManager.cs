using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    public Sound[] music;
    public Sound[] sfx;

    public float music_volume = 0.1f;
    public float sfx_volume = 0.1f;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Awake(){
        DontDestroyOnLoad(gameObject);

        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }

        /*foreach (Sound sound in music){
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        foreach (Sound sound in sfx){
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }*/
    }

    void Start(){
        PlayMusic("Main_Theme");
    }

    public void PlayMusic(string name){
        foreach (Sound sound in music){
            if (sound.name == name){
                musicSource.clip = sound.clip;
                musicSource.volume = sound.volume;
                musicSource.pitch = sound.pitch;
                musicSource.loop = sound.loop;
                musicSource.Play();

                return;
            }
        }

        Debug.Log("Sound '" + name + "' not found");
    }

    public void PlaySFX(string name, GameObject obj){
        foreach (Sound sound in sfx){
            if (sound.name == name){
                AudioSource sfxSource_ = Instantiate(sfxSource, obj.transform);

                sfxSource_.clip = sound.clip;
                sfxSource_.volume = sfx_volume;
                sfxSource_.pitch = sound.pitch;
                sfxSource_.loop = sound.loop;

                sfxSource_.Play();

                return;
            }
        }

        Debug.Log("Sound '" + name + "' not found");
    }

    public void MusicVolume(float volume){
        musicSource.volume = volume;
        music_volume = volume;
    }

    public void SFXVolume(float volume){
        sfx_volume = volume;
    }

}