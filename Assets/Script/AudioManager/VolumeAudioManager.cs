using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeAudioManager : MonoBehaviour
{
  
    void Start()
    {
        float volume = PlayerPrefs.GetFloat("setVolume", 0.5f);

        AudioListener.volume = volume;
    }
}
