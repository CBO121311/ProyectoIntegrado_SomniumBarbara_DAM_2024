using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : Menu
{
    /*public LogicVolume volumeController;
    public LogicFullScreen fullScreenController;
    public LogicQuality qualityController;*/

    private void Start()
    {
        // Inicializa cada controlador de configuración
        /*volumeController.Initialize();
        fullScreenController.Initialize();
        qualityController.Initialize();*/
    }

    // Método para guardar todas las configuraciones
    public void SaveSettings()
    {
        /*volumeController.SaveVolumePreference();
        fullScreenController.SaveScreenPreference();
        qualityController.SaveQualityPreference();*/
    }

    // Método para cargar todas las configuraciones
    public void LoadSettings()
    {
        //volumeController.LoadVolume();
        //fullScreenController.LoadFullScreen();
        //qualityController.LoadQualityLevel();
    }
}
