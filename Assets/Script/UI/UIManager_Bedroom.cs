using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager_Bedroom : MonoBehaviour, IDataPersistence
{

    [SerializeField] private TextMeshProUGUI daysGameText;

    [Header("Inventory")]
    private Inventory inventory;

    [Header("Pause Game")]
    [SerializeField] private GameObject panelPause;
    private PauseMenuSL pauseMenu;

    [Header("Cooldown")]
    private float cooldownInventory = 0.5f;
    private bool isCooldown = false;
    private float cooldownPause = 0.1f;


    private static UIManager_Bedroom instance;
    public bool inventoryIsActivated { get; private set; }
    public bool gameIsPaused { get; private set; }



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
        inventory = FindFirstObjectByType<Inventory>();

        UpdateDaysGameUI();

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

    public void UpdateDaysGameUI()
    {
    }

    public void LoadData(GameData data)
    {
        daysGameText.text = data.daysGame.ToString();
    }

    public void SaveData(GameData data)
    {
        
    }
}
