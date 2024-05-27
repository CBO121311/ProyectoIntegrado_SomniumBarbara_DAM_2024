using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager: Menu
{

    [Header("Quality Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("Fullscreen Settings")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown resolutionDropDown;
    private Resolution[] resolutions;

    [Header("Volume Settings")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image muteImage;
    public static SettingsManager instance { get; private set; }

    [SerializeField] private SettingTransition settingTransition;

    public event Action OnSettingsOpened;
    public event Action OnSettingsClosed;

    private void Awake()
    {

        if (instance != null)
        {
            Debug.Log("Se encontró más de un administrador de Configuración en la escena. Destruyendo el más nuevo.");
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        LoadSettings();
    }


    public void LoadSettings()
    {
        // Load Quality Settings
        int quality = PlayerPrefs.GetInt("setQuality", 2);

        if(qualityDropdown!= null)
        {
            qualityDropdown.value = quality;
        }
        AdjustQuality();


        // Load Fullscreen Settings
        bool isFullscreen = PlayerPrefs.GetInt("setFullscreen", 1) == 1;

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = isFullscreen;
        }

        Screen.fullScreen = isFullscreen;

        // Load Resolution Settings
        LoadResolutions();
        int resolutionIndex = PlayerPrefs.GetInt("setResolution", 0);

        if (resolutionDropDown != null)
        {
            resolutionDropDown.value = resolutionIndex;
        }
        ChangeResolution(resolutionIndex);

        // Load Volume Settings
        float volume = PlayerPrefs.GetFloat("setVolume", 0.5f);


        if (volumeSlider != null)
        {
            volumeSlider.value = volume;
        }      
        ChangeVolume(volume);
    }

    public void SaveSettings()
    {
        SaveQualityPreference();
        SaveScreenPreference();
        SaveVolumePreference();
    }

    // Quality Settings Methods
    public void AdjustQuality()
    {
        int quality = qualityDropdown.value;
        PlayerPrefs.SetInt("setQuality", quality);
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
        //Debug.Log("La calidad actual es: " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
    }

    private void SaveQualityPreference()
    {
        PlayerPrefs.SetInt("setQuality", qualityDropdown.value);
    }

    // Fullscreen and Resolution Settings Methods
    public void ActivateFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("setFullscreen", isFullscreen ? 1 : 0);
    }

    public void LoadResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();
        HashSet<string> uniqueResolutions = new HashSet<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (uniqueResolutions.Add(option))
            {
                options.Add(option);
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.RefreshShownValue();
    }

    public void ChangeResolution(int index)
    {
        string[] dimensions = resolutionDropDown.options[index].text.Split('x');
        int width = int.Parse(dimensions[0].Trim());
        int height = int.Parse(dimensions[1].Trim());

        Resolution resolution = Array.Find(resolutions, r => r.width == width && r.height == height);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("setResolution", index);
        //Debug.Log("Resolución cambiada a: " + resolution.width + " x " + resolution.height);
    }

    private void SaveScreenPreference()
    {
        PlayerPrefs.SetInt("setResolution", resolutionDropDown.value);
        PlayerPrefs.SetInt("setFullscreen", Screen.fullScreen ? 1 : 0);
    }

    // Volume Settings Methods
    public void ChangeVolume(float value)
    {
        PlayerPrefs.SetFloat("setVolume", value);
        AudioListener.volume = value;
        ToggleMuteImage(value);
    }

    private void SaveVolumePreference()
    {
        PlayerPrefs.SetFloat("setVolume", volumeSlider.value);
    }

    private void ToggleMuteImage(float value)
    {
        muteImage.enabled = value == 0;
    }

    //Método que abre la configuracion
    public void OpenSetting()
    {
        settingTransition.OpenSettingLevel();
        this.SetFirstSelected(volumeSlider);
        OnSettingsOpened?.Invoke();
    }

    //Método que cierra la configuracion
    public void CloseSetting()
    {
        settingTransition.CloseSettingLevel();
        OnSettingsClosed?.Invoke(); 
    }
}
