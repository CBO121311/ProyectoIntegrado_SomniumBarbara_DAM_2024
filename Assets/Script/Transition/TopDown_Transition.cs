using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopDown_Transition : MonoBehaviour
{
    [SerializeField] private GameObject panelDays;
    [SerializeField] private GameObject pauseMenu;
    Vector3 initPosPauseMenu, initPosInventoryLevel;
    [SerializeField] private GameObject fadeGameobject;
    private Image fadeImage;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject levelInfoPanel;
    [SerializeField] private GameObject inventoryLevel;

    private void Awake()
    {
        if (!fadeGameobject.activeSelf)
        {
            fadeGameobject.SetActive(true);
        }

        if (fadeGameobject != null)
        {
            fadeImage = fadeGameobject.GetComponent<Image>();
        }

        if (inventoryLevel != null)
        {
            initPosInventoryLevel = inventoryLevel.GetComponent<RectTransform>().anchoredPosition;
        }

        if (pauseMenu != null)
        {
            initPosPauseMenu = pauseMenu.GetComponent<RectTransform>().anchoredPosition;
        }

    }

    private void Start()
    {

        FadeIn();
    }

    //Animación al empezar escena
    public void FadeIn()
    {
        
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
        LeanTween.alpha(fadeImage.rectTransform, 0f, 1f)
            .setEase(LeanTweenType.easeInOutQuad);

        if(audioSource != null)
        {
            LeanTween.delayedCall(0.5f, () =>
            {
                audioSource.Play();
            });
        }
    }

    //Animación al salir de la escena.
    public void FadeOutAndLoadScene(string sceneName)
    {
        LeanTween.alpha(fadeImage.rectTransform, 1f, 1f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() => SceneManager.LoadScene(sceneName));
    }

    //Animación al mostrar la info del nivel.
    public void ShowLevelInfoPanel()
    {
        LeanTween.alphaCanvas(levelInfoPanel.GetComponent<CanvasGroup>(), 1f, 0.6f)
            .setEase(LeanTweenType.easeOutQuad);
    }

    //Animación para ocultar la info del nivel.
    public void HideLevelInfoPanel()
    {
        LeanTween.alphaCanvas(levelInfoPanel.GetComponent<CanvasGroup>(), 0f, 0.6f)
            .setEase(LeanTweenType.easeOutQuad);
    }

    //Animación al abrir el menú pause
    public void OpenPauseMenu()
    {
        LeanTween.moveX(pauseMenu.GetComponent<RectTransform>(), 0, 0.3f)
            .setEaseInOutQuad();
    }

    public void ClosePauseMenu()
    {
        LeanTween.moveX(pauseMenu.GetComponent<RectTransform>(), initPosPauseMenu.x, 0.3f)
            .setEaseInOutQuad()
            .setOnComplete(() => pauseMenu.SetActive(false));
    }

    //Animación al abrir el inventario
    public void OpenInventory()
    {
        Time.timeScale = 0;
        LeanTween.moveY(inventoryLevel.GetComponent<RectTransform>(), 0, 0.5f)
            .setEaseOutQuint()
            .setIgnoreTimeScale(true);
    }

    public void CloseInventory()
    {
        LeanTween.moveY(inventoryLevel.GetComponent<RectTransform>(), initPosInventoryLevel.y, 0.5f)
            .setEaseOutQuint()
            .setIgnoreTimeScale(true)
            .setOnComplete(() => Time.timeScale = 1);
    }
}
