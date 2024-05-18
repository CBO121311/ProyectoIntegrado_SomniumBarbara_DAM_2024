using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : Menu
{
    [Header("Quality Settings")]
    public TMP_Dropdown qualityDropdown;

    [Header("Fullscreen Settings")]
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropDown;
    private Resolution[] resolutions;

    [Header("Volume Settings")]
    public Slider volumeSlider;
    public Image muteImage;

    private void Start()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        // Load Quality Settings
        int quality = PlayerPrefs.GetInt("setQuality", 2);
        qualityDropdown.value = quality;
        AdjustQuality();

        // Load Fullscreen Settings
        bool isFullscreen = PlayerPrefs.GetInt("setFullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        // Load Resolution Settings
        LoadResolutions();
        int resolutionIndex = PlayerPrefs.GetInt("setResolution", 0);
        resolutionDropDown.value = resolutionIndex;
        ChangeResolution(resolutionIndex);

        // Load Volume Settings
        float volume = PlayerPrefs.GetFloat("setVolume", 0.5f);
        volumeSlider.value = volume;
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
                QualitySettings.SetQualityLevel(0); // "Low" in Unity
                break;
            case 1:
                QualitySettings.SetQualityLevel(2); // "Medium" in Unity
                break;
            case 2:
                QualitySettings.SetQualityLevel(5); // "Ultra" in Unity
                break;
        }
        Debug.Log("La calidad actual es: " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
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

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.RefreshShownValue();
    }

    public void ChangeResolution(int index)
    {
        PlayerPrefs.SetInt("setResolution", resolutionDropDown.value);
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log("Resolución cambiada a: " + resolution.width + " x " + resolution.height);
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

    // Ensure settings are applied when changing scenes
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadSettings();
    }
}
