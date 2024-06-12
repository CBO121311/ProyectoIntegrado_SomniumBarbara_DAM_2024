using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager_TopDown : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI daysGameText;

    [Header("Inventory")]
    [SerializeField] private Inventory inventory;


    [Header("Level Game")]
    [SerializeField] private GameObject levelInfoPanel;
    private LevelSelection levelSelection;

    [Header("Pause Game")]
    [SerializeField] private GameObject panelPause;
    private PauseMenuSL pauseMenu;

    [Header("Cooldown")]
    private float cooldownInventory = 0.5f;
    private bool isCooldown = false;
    private float cooldownPause = 0.1f;


    private static UIManager_TopDown instance;
    public bool inventoryIsActivated { get; private set; }
    public bool levelSelectionIsActive { get; private set; }
    public bool gameIsPaused { get; private set; }

    //private TimeManager timeManager;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Hay más de un UIManager_TopDown en la Escena");
            Destroy(gameObject);
            return;
        }

        instance = this;
        gameIsPaused = false;
    }

    private void Start()
    {
        //timeManager = FindFirstObjectByType<TimeManager>();
        pauseMenu = panelPause.GetComponent<PauseMenuSL>();

        if(levelInfoPanel != null)
        {
            levelSelection = levelInfoPanel.GetComponentInChildren<LevelSelection>();
        }

        if (daysGameText != null)
        {
            daysGameText.text = TimeManager.currentDay.ToString();
        }
    }
   

    public static UIManager_TopDown GetInstance()
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

    public void ToggleInventoryUI()
    {
        if (isCooldown) return;

        inventoryIsActivated = !inventoryIsActivated;

        if (inventoryIsActivated)
        {
            inventory.ToogleInventory();
        }
        else
        {
            inventory.ToogleInventory();
        }

        StartCoroutine(CooldownCoroutine(cooldownInventory));
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
