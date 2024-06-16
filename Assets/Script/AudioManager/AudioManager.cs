using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip[] backgroundMusicClips;
    public AudioClip[] sfxClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("setVolume", 0.5f);

        AudioListener.volume = volume;
    }

    public void PlayMusic(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= backgroundMusicClips.Length) return;
        musicSource.clip = backgroundMusicClips[clipIndex];
        musicSource.Play();
    }

    public void PlaySFX(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= sfxClips.Length) return;
        sfxSource.PlayOneShot(sfxClips[clipIndex]);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenuUI" || scene.name == "BedroomScene" || scene.name == "SquirrelLevel" ||  scene.name == "BunnyLevel" || scene.name == "LevelSelection")
        {
            StopMusic();
        }
        else
        {
            //Debug.Log("Intentando parar música");
        }
    }
}
