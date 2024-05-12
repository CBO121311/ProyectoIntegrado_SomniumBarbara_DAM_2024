using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    [SerializeField] private AudioSource normalBGM;
    [SerializeField] private AudioSource terrorBGM;


    [SerializeField] private AudioSource audioSourceSFX;

    [SerializeField] private AudioClip[] audioClips;
    private Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>();


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Se encontró más de un AudioManager en la escena.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(this.gameObject);

        // Precarga todos los clips de audio y los almacena en el diccionario
        foreach (var clip in audioClips)
        {
            audioClipDict.Add(clip.name, clip);
        }

        //audioSourceBGM = GetComponent<AudioSource>();
    }

    // Método para obtener un clip de audio precargado por su nombre
    
    public AudioClip GetAudioClip(string clipName)
    {
        if (audioClipDict.ContainsKey(clipName))
        {
            return audioClipDict[clipName];
        }
        else
        {
            Debug.LogWarning($"El clip de audio '{clipName}' no fue encontrado en la lista de precarga.");
            return null;
        }
    }

    // Método para reproducir una pista de música por su nombre

    /*
    public void PlayMusic(string clipName)
    {
        if (audioClipDict.ContainsKey(clipName))
        {
            normalBGM.clip = audioClipDict[clipName];
            normalBGM.Play();
        }
        else
        {
            Debug.LogWarning($"El clip de audio '{clipName}' no fue encontrado en la lista de precarga.");
        }
    }*/

    // Método para cambiar la música (por ejemplo, al presionar un botón)
    public void ChangeMusicTerror()
    {
        normalBGM.Stop(); 
        terrorBGM.Play();
    }
    public void PlayOneSound(string clipName)
    {
        audioSourceSFX.PlayOneShot(GetAudioClip(clipName));
    }
}
