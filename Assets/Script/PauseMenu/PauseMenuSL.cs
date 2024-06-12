using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuSL : Menu
{
    private TimeManager timeManager;

    [Header("Menu Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject panelGameSaved;
    [SerializeField] private TopDown_Transition transition;

    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopMenu confirmationPopMenu;
    private bool pauseIsEnable;

    private bool isSettingOpen = false;

    private void Start()
    {
        timeManager = FindFirstObjectByType<TimeManager>();

        if(SettingsManager.instance != null)
        {
            SettingsManager.instance.OnSettingsClosed += OnSettingClosed;
            SettingsManager.instance.OnSettingsOpened += OnSettingOpened;
        }
    }

    private void OnDestroy()
    {
        if (SettingsManager.instance != null)
        {
            SettingsManager.instance.OnSettingsClosed -= OnSettingClosed;
            SettingsManager.instance.OnSettingsOpened -= OnSettingOpened;
        }
    }

    //Activa la corrutina para abrir el menú de pause

    public bool IsSettingOpen()
    {
        return isSettingOpen;
    }
    
    private void OnSettingOpened()
    {
        isSettingOpen = true;
    }

    private void OnSettingClosed()
    {
        isSettingOpen = false;
        StartCoroutine(HideSettingPanel());
    }


    public void TooglePause()
    {
        pauseIsEnable = !pauseIsEnable;

        if (pauseIsEnable)
        {
            SetUpOptionMenu();
        }
        else
        {
            OnBackClicked();
        }
    }

    public void SetUpOptionMenu()
    {
        StartCoroutine(ShowOptionPanel());
    }

    //Activa la corrutina para cerrar el menú de pause
    public void OnBackClicked()
    {
        StartCoroutine(HideOptionPanel());
    }

    //Guarda la partida
    public void OnSaveClicked()
    {
        DataPersistenceManager.instance.SaveGame();

        StartCoroutine(DisableSaveButtonForSeconds(1f));
    }

    //Abre setting
    public void OpenSetting()
    {
        SettingsManager.instance.OpenSetting();
    }


    //Activa la interactuación de los botones
    private void ActivateMenuButtons()
    {
        resumeButton.interactable = true;
        saveButton.interactable = true;
        settingButton.interactable = true;
        exitButton.interactable = true;
    }

    //Desactiva la interactuación de los botones
    private void DisableMenuButtons()
    {
        resumeButton.interactable = false;
        saveButton.interactable = false;
        settingButton.interactable = false;
        exitButton.interactable = false;
    }

    public void GoMainMenu()
    {
        confirmationPopMenu.ActivateMenu("¿Estás seguro que quieres volver al menú principal?\n\n" +
            "Perderás los datos no guardados.",
            //Función que se ejecuta si seleccionamos "Confirmar"
            () =>
            {
                timeManager.StopGame();
                transition.FadeOutAndLoadScene("MainMenuUI");
            },

            //Función que se ejecuta si seleccionamos "Cancelar"
            () =>
            {
                Debug.Log("Cancelar salir del juego");
                //Establecer el primer botón seleccionado.
                this.SetFirstSelected(exitButton);
            }
            );
    }

    //Muestra el menú de pauseSL
    private IEnumerator ShowOptionPanel()
    {
        ActivateMenuButtons();
        transition.OpenPauseMenu();
        yield return new WaitForSeconds(0.5f);
    }

    //Oculta el menú de pauseSL
    private IEnumerator HideOptionPanel()
    {
        DisableMenuButtons();
        transition.ClosePauseMenu();
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

    //Oculta el menú setting
    private IEnumerator HideSettingPanel()
    {
        yield return new WaitForSeconds(0.6f);
        this.SetFirstSelected(settingButton);
    }

    // Desactiva el botón de guardar mediante un parámetro
    private IEnumerator DisableSaveButtonForSeconds(float seconds)
    {
        panelGameSaved.SetActive(true);
        DisableMenuButtons();
        yield return new WaitForSeconds(seconds);
        ActivateMenuButtons();
        panelGameSaved.SetActive(false);
        this.SetFirstSelected(resumeButton);
    }
}
