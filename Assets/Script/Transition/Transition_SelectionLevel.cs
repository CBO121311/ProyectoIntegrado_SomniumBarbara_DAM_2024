using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition_SelectionLevel : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject inventoryLevel;
    Vector3 initPosPauseMenu, initPosInventoryLevel;
    [SerializeField] private GameObject fadeGameobject;
    private Image fadeImage;


    private void Awake()
    {
        if (!fadeGameobject.activeSelf)
        {
            fadeGameobject.SetActive(true);
        }

        fadeImage = fadeGameobject.GetComponent<Image>();
        initPosPauseMenu = pauseMenu.GetComponent<RectTransform>().anchoredPosition;
        initPosInventoryLevel = inventoryLevel.GetComponent<RectTransform>().anchoredPosition;
    }

    private void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
        LeanTween.alpha(fadeImage.rectTransform, 0f, 1f).setEase(LeanTweenType.easeInOutQuad);
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        LeanTween.alpha(fadeImage.rectTransform, 1f, 1f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() => SceneManager.LoadScene(sceneName));
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
