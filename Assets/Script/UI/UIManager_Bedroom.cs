using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_Bedroom : MonoBehaviour
{
    public bool gameIsPaused { get; private set; }

    private static UIManager_Bedroom instance;

    [Header("Pause Game")]
    [SerializeField] private GameObject panelPause;
    private PauseMenuSL pauseMenu;

    [Header("Cooldown")]
    private bool isCooldown = false;
    private float cooldownPause = 0.1f;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Hay más de un Bedroom_UIManager en la Escena");
            Destroy(gameObject);
            return;
        }

        instance = this;
        gameIsPaused = false;
    }

    private void Start()
    {
        pauseMenu = panelPause.GetComponent<PauseMenuSL>();
    }

    public static UIManager_Bedroom GetInstance()
    {
        return instance;
    }

    // Activar/desactivar la UI del inventario según sea necesario
    public void TogglePauseUI()
    {
        if (isCooldown || pauseMenu.IsSettingOpen()) return;

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

        StartCoroutine(CooldownCoroutine(cooldownPause));
    }

    private IEnumerator CooldownCoroutine(float cooldown)
    {
        isCooldown = true;
        yield return new WaitForSecondsRealtime(cooldown);
        isCooldown = false;
    }
}
