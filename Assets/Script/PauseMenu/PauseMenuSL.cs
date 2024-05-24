using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuSL : Menu, IPauseMenu
{
    [Header("Menu Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject panelGameSaved;
    [SerializeField] private SLTransition slTransition;

    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopMenu confirmationPopMenu;

    private void Start()
    {
        if(SettingsManager.instance != null)
        {
            SettingsManager.instance.OnSettingsClosed += OnSettingClosed;
        }
    }
    private void OnDestroy()
    {
        if (SettingsManager.instance != null)
        {
            SettingsManager.instance.OnSettingsClosed -= OnSettingClosed;
        }
    }

    public void SetUpOptionMenu()
    {
        StartCoroutine(ShowOptionPanel());
    }

    public void OnBackClicked()
    {
        StartCoroutine(HideOptionPanel());
    }

    public void OnSaveClicked()
    {
        DataPersistenceManager.instance.SaveGame();

        StartCoroutine(DisableSaveButtonForSeconds(1f));
    }

    public void OpenSetting()
    {
        SettingsManager.instance.OpenSetting();
    }
    public void OnSettingClosed()
    {
        StartCoroutine(HideSettingPanel());
    }

    private void ActivateMenuButtons()
    {
        resumeButton.interactable = true;
        saveButton.interactable = true;
        settingButton.interactable = true;
        exitButton.interactable = true;
    }

    private void DisableMenuButtons()
    {
        resumeButton.interactable = false;
        saveButton.interactable = false;
        settingButton.interactable = false;
        exitButton.interactable = false;
    }

    public void GoMainMenu()
    {
        //DisableMenuButtons();
        confirmationPopMenu.ActivateMenu("¿Estás seguro que quieres volver al menú principal?\n\n" +
            "Perderás los datos no guardados.",
            //Función que se ejecuta si seleccionamos "Confirmar"
            () =>
            {
                SceneManager.LoadSceneAsync("MainMenuUI");
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
        slTransition.OpenPauseMenu();
        yield return new WaitForSeconds(0.5f);
    }

    //Oculta el menú de pauseSL
    private IEnumerator HideOptionPanel()
    {
        DisableMenuButtons();
        slTransition.ClosePauseMenu();
        yield return new WaitForSeconds(0.5f);
        UIManager.changeGameIsPaused();
        this.gameObject.SetActive(false);
    }

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
