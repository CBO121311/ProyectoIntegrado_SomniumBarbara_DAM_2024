using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicFullScreen : MonoBehaviour
{

    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropDown;
    Resolution[] resolutions;

    private void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
        LoadResolution();
    }

    public void ActivateFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void LoadResolution()
    {

        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();


        int resolucionActual = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionOption = resolutions[i].width + " x " + resolutions[i].height;


            if (options.Contains(resolutionOption)) continue;

            options.Add(resolutionOption);

            if (Screen.fullScreen && resolutions[i].width ==
            Screen.currentResolution.width && resolutions[i].height ==
            Screen.currentResolution.height)
            {
                resolucionActual = i;
            }

        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = resolucionActual;
        resolutionDropDown.RefreshShownValue();

        resolutionDropDown.value = PlayerPrefs.GetInt("setResolution", 0);
    }

    public void ChangeResolution(int indexResolution)
    {
        PlayerPrefs.SetInt("setResolution", resolutionDropDown.value);
        Resolution resolution = resolutions[indexResolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        Debug.Log("Resolución cambio a: " + resolution.width + " x " + resolution.height);
    }


    public void SaveScreenPreference()
    {
        PlayerPrefs.SetInt("setResolution", resolutionDropDown.value);
    }
}
