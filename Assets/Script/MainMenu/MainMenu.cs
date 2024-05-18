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

    [SerializeField]GameObject panelSetting;

    [SerializeField] private Button backButtonSetting;

    [SerializeField] RectTransform fader;
    Animator animator;
    private bool setting = false;


    private void Start()
    {
        animator = panelSetting.GetComponent<Animator>();
        DisableButtonsDependingOnData();
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
            //guarda el juego en cualquier momento antes de cargar una nueva escena
            //DataPersistenceManager.instance.SaveGame();

            SceneManager.LoadSceneAsync("LevelSelection");
        }));
    }

    public void SettingGame()
    {
        //fader.gameObject.SetActive(true);
        
        if (!setting)
        {
            OpenSettingMenu();
            //Invoke("OpenSettingMenu", 0.5f);
        }
        else
        {
            CloseSettingMenu();
        }    
    }
    private void OpenSettingMenu()
    {
        panelSetting.SetActive(true);
        animator.SetBool("showSetting",true);
        setting = true;
        this.SetFirstSelected(backButtonSetting);
    }
    private void CloseSettingMenu()
    {
        animator.SetBool("showSetting", false);
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
        panelSetting.SetActive(false);
        setting = false;
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
