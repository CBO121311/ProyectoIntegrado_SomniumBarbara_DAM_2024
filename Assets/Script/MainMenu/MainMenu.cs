using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button loadGameButton;

    [Header("Transition")]
    [SerializeField] private MainMenuTransition mainMenuTransition;
    public static MainMenu Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {

        DisableButtonsDependingOnData();

        if (SettingsManager.instance != null)
        {
            SettingsManager.instance.OnSettingsClosed += OnSettingClosed;
        }

        PlayerState.Allfalse();
    }
    private void OnDestroy()
    {
        if (SettingsManager.instance != null)
        {
            SettingsManager.instance.OnSettingsClosed -= OnSettingClosed;
        }
    }

    private void DisableButtonsDependingOnData()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }


    public void OnNewGameClicked()
    {
        StartCoroutine(DelayedAction(0.2f,()=>
        {
            saveSlotsMenu.ActivateMenu(false);
            this.DeactivateMenu();
        }));
    }


    public void OnLoadGameClicked()
    {
        StartCoroutine(DelayedAction(0.2f, () =>
        {
            saveSlotsMenu.ActivateMenu(true);
            this.DeactivateMenu();
        }));
    }
    public void OnContinueGameClicked()
    {

        StartCoroutine(DelayedAction(0.2f, () =>
        {
            DisableMenuButtons();
            mainMenuTransition.FadeOutAndLoadScene("BedroomScene");
        }));
    }

    //Método que abre el menu Setting
    public void OpenSettingMenu()
    {
        SettingsManager.instance.OpenSetting();
    }

    //Método realiza la corrutina al salir de Setting.
    public void OnSettingClosed()
    {
        StartCoroutine(HideSettingPanel());
    }


    public void QuitGameScene()
    {
        StartCoroutine(DelayedAction(0.2f, () =>
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }));
    }

    private IEnumerator HideSettingPanel()
    {
        yield return new WaitForSeconds(0.6f);
        this.SetFirstSelected(settingButton);
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }
    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

}
