using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_SelectionLevel : MonoBehaviour
{
    public bool gameIsPaused { get; private set; }
    public bool levelSelectionIsActive { get; private set; }

    private static UIManager_SelectionLevel instance;

    [Header("Pause Game")]
    [SerializeField] private GameObject panelPause;
    private PauseMenuSL pauseMenu;


    [Header("Level Game")]
    [SerializeField] private GameObject levelInfoPanel;
    private LevelSelection levelSelection;

    [Header("Cooldown")]
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
        gameIsPaused = false;
    }

    private void Start()
    {
        pauseMenu = panelPause.GetComponent<PauseMenuSL>();
        levelSelection = levelInfoPanel.GetComponentInChildren<LevelSelection>();
    }

    public static UIManager_SelectionLevel GetInstance()
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


    public void ActivateSelectLevel()
    {
        levelInfoPanel.SetActive(true);
        levelSelectionIsActive = true;
    }

    public void DisableSelectLevel()
    {
        levelSelectionIsActive = false;
        levelInfoPanel.SetActive(false);
    }
}
