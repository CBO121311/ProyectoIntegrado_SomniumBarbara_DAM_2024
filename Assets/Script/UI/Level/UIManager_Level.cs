using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_Level : MonoBehaviour
{

    [Header("Pause Game")]
    [SerializeField] private GameObject panelPause;
    private PauseMenuLevel pauseMenu;
    public bool gameIsPaused { get; private set; }

    private bool isCooldown = false;
    private float pauseCooldown = 0.2f;

    private static UIManager_Level instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Hay más de un SelectionLevel_UIManager en la escena. Se destruye el más nuevo");
            Destroy(gameObject);
            return;
        }

        instance = this;
        gameIsPaused = false;
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
        if (isCooldown) return;

        gameIsPaused = !gameIsPaused;


        if (gameIsPaused)
        {
            Debug.Log("Pause true");
            panelPause.SetActive(true);
            pauseMenu.TooglePause();
        }
        else
        {
            Debug.Log("Pause false");
            pauseMenu.TooglePause();
        }

        StartCoroutine(CooldownCoroutine(pauseCooldown));
    }

    private IEnumerator CooldownCoroutine(float cooldown)
    {
        isCooldown = true;
        yield return new WaitForSecondsRealtime(cooldown);
        isCooldown = false;
    }

    public bool IsPauseCooldownActive()
    {
        return isCooldown;
    }
}
