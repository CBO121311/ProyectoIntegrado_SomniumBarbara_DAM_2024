using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_Level : MonoBehaviour
{
    private static UIManager_Level instance;

    [Header("Pause Game")]
    [SerializeField] private GameObject panelPause;
    private PauseMenuLevel pauseMenu;

    private bool isCooldown = false;
    private float cooldownPause = 0.1f;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Hay más de un SelectionLevel_UIManager en la Escena");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        pauseMenu = panelPause.GetComponent<PauseMenuLevel>();
    }

    public static UIManager_Level GetInstance()
    {
        return instance;
    }

    // Activar/desactivar la UI del inventario según sea necesario
    public void TogglePauseUI()
    {
        /*if (isCooldown || pauseMenu.IsSettingOpen()) return;

        gameIsPaused = !gameIsPaused;


        if (gameIsPaused)
        {
            panelPause.SetActive(true);
            pauseMenu.TooglePause();
        }
        else
        {
            pauseMenu.TooglePause();
        }

        StartCoroutine(CooldownCoroutinePause());*/
    }

    private IEnumerator CooldownCoroutinePause()
    {
        isCooldown = true;
        yield return new WaitForSecondsRealtime(cooldownPause);
        isCooldown = false;
    }
}
