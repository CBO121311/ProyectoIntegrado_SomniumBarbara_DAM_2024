using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_SelectionLevel : MonoBehaviour
{
    public bool gameIsPaused { get; private set; }
    public bool inventoryIsActivated { get; private set; }
    private static UIManager_SelectionLevel instance;

    [Header("Inventory")]
    [SerializeField] private GameObject panelInventory;
    private Inventory inventory;

    [Header("Pause Game")]
    [SerializeField] private GameObject panelPause;
    private PauseMenuSL pauseMenu;

    [Header("Cooldown")]
    private bool isCooldown = false;
    private float cooldownInventory = 0.5f;
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
        inventoryIsActivated = false;
    }

    private void Start()
    {
        pauseMenu = panelPause.GetComponent<PauseMenuSL>();
        inventory = panelInventory.GetComponent<Inventory>();
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

        StartCoroutine(CooldownCoroutinePause());
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

        StartCoroutine(CooldownCoroutineInventory());
    }
    private IEnumerator CooldownCoroutineInventory()
    {
        isCooldown = true;
        yield return new WaitForSecondsRealtime(cooldownInventory);
        isCooldown = false;
    }

    private IEnumerator CooldownCoroutinePause()
    {
        isCooldown = true;
        yield return new WaitForSecondsRealtime(cooldownPause);
        isCooldown = false;
    }
}
