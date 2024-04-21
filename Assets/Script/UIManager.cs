using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject optionMenuGameObject;
    [SerializeField] GameObject fadeSL;
    [SerializeField] OptionMenu optionMenu;
    public static bool gameIsPaused { get; private set; }
    private bool previousPauseState = false;

    private void Awake()
    {
        
        StartCoroutine(fadeSl());
        gameIsPaused = false;
        if (optionMenuGameObject.activeSelf) 
        {
            optionMenuGameObject.SetActive(false);
        }
    }

    IEnumerator fadeSl()
    {
        fadeSL.SetActive(true);
        yield return new WaitForSeconds(0.80f);
        fadeSL.SetActive(false);
    }

    void Update()
    {
        if (gameIsPaused && !previousPauseState)
        {
            Pause();
            previousPauseState = true;
        }
        else if (!gameIsPaused && previousPauseState)
        {
            // Realizar cualquier acción necesaria cuando el juego se reanude
            previousPauseState = false;
        }
    }

    public void Pause()
    {
        optionMenuGameObject.SetActive(true);
        optionMenu.SetUpOptionMenu();
    }
    public static void changeValueGameIsPaused()
    {
        gameIsPaused = !gameIsPaused;
    }
}
