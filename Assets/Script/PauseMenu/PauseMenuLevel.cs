using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuLevel : Menu, IPauseMenu
{
    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button exitButton;

    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopMenu confirmationPopMenu;

    public void SetUpOptionMenu()
    {
        ActivateMenuButtons();
        StartCoroutine(ShowOptionPanel());
    }

    public void OnBackClicked()
    {
        DisableMenuButtons();
        StartCoroutine(HideOptionPanel());
    }

    //Muestra el menú de pause
    private IEnumerator ShowOptionPanel()
    {
        Time.timeScale = 0f;
        //this.animator.SetBool("Pause", true);
        yield return new WaitForSecondsRealtime(0.5f);

        //Debug.Log("Mostrar Menú");
    }

    //Oculta el menú de pauseSL
    private IEnumerator HideOptionPanel()
    {
        DisableMenuButtons();
        // this.animator.SetBool("Pause", false);
        Time.timeScale = 1.0f;
        yield return new WaitForSecondsRealtime(0.5f);
        UIManager.changeGameIsPaused();
        this.gameObject.SetActive(false);

        //Debug.Log("Apagar menú");
    }


    //Muestra el menú de pause 

    private void ActivateMenuButtons()
    {
        backButton.interactable = true;
        exitButton.interactable = true;
    }

    private void DisableMenuButtons()
    {
        backButton.interactable = false;
        exitButton.interactable = false;
    }

    public void GoLevelSelection()
    {
        confirmationPopMenu.ActivateMenu("¿Estás seguro que quieres salir de la pantalla?\n\n" +
            "Perderás los objetos recogidos.",
            () =>
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadSceneAsync(1);
            },

            () =>
            {
                this.SetFirstSelected(exitButton);
            });
    }
}
