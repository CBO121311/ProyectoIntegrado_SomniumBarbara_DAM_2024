using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("setVolume", 0.5f);
        switch (PlayerPrefs.GetInt("setQuality", 2))
        {
            case 0:
                QualitySettings.SetQualityLevel(0); // "Low" en Unity
                break;
            case 1:
                QualitySettings.SetQualityLevel(2); // "Medium" en Unity
                break;
            case 2:
                QualitySettings.SetQualityLevel(5); // "Ultra" en Unity
                break;
        }


    }
}
