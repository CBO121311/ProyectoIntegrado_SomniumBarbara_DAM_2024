using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_SelectionLevel : MonoBehaviour
{
    public bool gameIsPaused { get; private set; }
    public bool inventoryIsActivated { get; private set; }
    //private LevelSquirrelTransition levelTransition;
    private static UIManager_SelectionLevel instance;

    [Header("Inventory")]
    [SerializeField] private GameObject panelInventory;
    private Inventory inventory;

    [Header("Pause Game")]
    [SerializeField] private GameObject panelPause;
    private PauseMenuSL pauseMenu;

    private bool isCooldown = false;
    [SerializeField] private float cooldownTime = 0.5f; // tiempo en segundos para el cooldown


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
        //levelTransition = FindFirstObjectByType<LevelSquirrelTransition>();
    }

    public static UIManager_SelectionLevel GetInstance()
    {
        return instance;
    }

    // Activar/desactivar la UI del inventario según sea necesario
    public void TogglePauseUI()
    {
        //Si setting está activado no se puede cerrar el menú de pause
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

        StartCoroutine(CooldownCoroutine());
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

        StartCoroutine(CooldownCoroutine());
    }
    private IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        yield return new WaitForSecondsRealtime(cooldownTime);
        isCooldown = false;
    }
}
