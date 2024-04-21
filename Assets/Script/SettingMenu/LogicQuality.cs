using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogicQuality : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown;
    private int quality;

    public void Initialize()
    {
        /*quality = PlayerPrefs.GetInt("setQuality", 2);
        qualityDropdown.value = quality;
        SetQualityLevel(); */
    }

    private void Start()
    {
        quality = PlayerPrefs.GetInt("setQuality", 2);
        qualityDropdown.value = quality;
        AdjustQuality();
    }

    public void AdjustQuality()
    {
        quality = qualityDropdown.value;
        PlayerPrefs.SetInt("setQuality", qualityDropdown.value);
        switch (quality)
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
        PlayerPrefs.SetInt("setQuality", qualityDropdown.value);

        // Obtiene el nombre de la calidad actual
        string currentQualityName = QualitySettings.names[QualitySettings.GetQualityLevel()];
        Debug.Log("La calidad actual es: " + currentQualityName);
    }

    public void SaveQualityPreference()
    {
        PlayerPrefs.SetInt("setQuality", qualityDropdown.value);
    }
}
