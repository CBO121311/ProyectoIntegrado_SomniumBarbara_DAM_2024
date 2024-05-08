using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject optionMenuGameObject;
    public IPauseMenu optionMenu;

    private static bool gameIsPaused;
    private bool previousPauseState = false;

    public static bool GameIsPaused { get => gameIsPaused;}
    private LevelTransition levelTransition;

    private void Awake()
    {
        gameIsPaused = false;
        if (optionMenuGameObject.activeSelf)
        {
            optionMenuGameObject.SetActive(false);
        }
       
        optionMenu = optionMenuGameObject.GetComponent<IPauseMenu>();
    }

    private void Start()
    {
        levelTransition = FindFirstObjectByType<LevelTransition>();
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

        if(levelTransition != null)
        {
            levelTransition.OpenPauseMenu();
        }     
        optionMenu.SetUpOptionMenu();
    }
    public static void changeGameIsPaused()
    {
        gameIsPaused = !gameIsPaused;
    }
}
