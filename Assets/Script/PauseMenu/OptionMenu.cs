using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionMenu : Menu
{
    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;
    public bool gameIsSetting = false;
    Animator animator;

    [SerializeField] private GameObject panelGameSaved;
    [SerializeField] private GameObject panelSetting;
    [SerializeField] private Animator settingMenuAnimator;


    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopMenu confirmationPopMenu;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //Debug.Log("OPTION MENU SE DESPERTÓ");
    }

    public void SetUpOptionMenu()
    {
        ActivateMenuButtons();
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

    public void SettingGame()
    {
        if (!gameIsSetting)
        {
            OpenSettingMenu();
        }
        else
        {
            CloseSettingMenu();
        }
    }

    private void OpenSettingMenu()
    {
        panelSetting.gameObject.SetActive(true);
        gameIsSetting = true;
        settingMenuAnimator.SetBool("showSetting", true);
    }
    /*private void CloseSettingMenu()
    {
        gameIsSetting = false;
        StartCoroutine(HideSettingPanel());

        this.SetFirstSelected(settingButton);
    }*/
    private void CloseSettingMenu()
    {
        settingMenuAnimator.SetBool("showSetting", false);
        StartCoroutine(HideSettingPanel());
    }

    private void ActivateMenuButtons()
    {
        backButton.interactable = true;
        saveButton.interactable = true;
        settingButton.interactable = true;
        exitButton.interactable = true;
    }

    private void DisableMenuButtons()
    {
        backButton.interactable = false;
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
                SceneManager.LoadSceneAsync(0);
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

    //Para el tema de las opcioens del menú
    private IEnumerator ShowOptionPanel()
    {
        this.animator.SetBool("showOption", true);
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("ShowOptionPanel");
    }

    private IEnumerator HideOptionPanel()
    {
        DisableMenuButtons();
        this.animator.SetBool("showOption", false);
        yield return new WaitForSeconds(0.5f);
        UIManager.changeValueGameIsPaused();
        this.gameObject.SetActive(false);
    }

    private IEnumerator HideSettingPanel()
    {
        yield return new WaitForSeconds(0.6f);
        panelSetting.SetActive(false);
        gameIsSetting = false;
        this.SetFirstSelected(settingButton);
    }

    /*private IEnumerator HideSettingPanel()
    {
        settingMenuAnimator.SetBool("showSetting", false);
        this.SetFirstSelected(settingButton);
        yield return new WaitForSeconds(0.5f);
        panelSetting.gameObject.SetActive(false);
    }*/



    // Desactiva el botón de guardar mediante un parámetro
    private IEnumerator DisableSaveButtonForSeconds(float seconds)
    {
        panelGameSaved.SetActive(true);
        DisableMenuButtons();
        yield return new WaitForSeconds(seconds);
        ActivateMenuButtons();
        panelGameSaved.SetActive(false);
        this.SetFirstSelected(backButton);
    }
}
