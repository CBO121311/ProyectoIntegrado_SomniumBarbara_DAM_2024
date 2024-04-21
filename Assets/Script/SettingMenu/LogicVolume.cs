using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicVolume : MonoBehaviour
{
    public Slider slider;
    private float sliderValue;
    public Image imagenMute;


    public void Initialize()
    {
        //Valor predefinido
        //slider.value = PlayerPrefs.GetFloat("setVolume", 0.5f);
        //AudioListener.volume = slider.value;
        //ToggleMuteImage();
    }

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("setVolume", 0.5f);
        //AudioListener.volume = slider.value;
        ToggleMuteImage();
    }

    //Cambia el valor y se guarda
    public void ChangeVolume(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("setVolume", sliderValue);
        AudioListener.volume = slider.value;
        ToggleMuteImage();
    }

    // Activa o desactiva la imagen de referencia de silencio
    public void ToggleMuteImage()
    {
        //Debug.Log(slider.value);

        if (slider.value == 0)
        {
            imagenMute.enabled = true;
        }
        else
        {
            imagenMute.enabled = false;
        }
    }

    public void SaveVolumePreference()
    {
        PlayerPrefs.SetFloat("setVolume", slider.value);
    }
}
