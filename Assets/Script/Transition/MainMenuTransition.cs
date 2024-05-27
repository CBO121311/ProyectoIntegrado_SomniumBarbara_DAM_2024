using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuTransition : MonoBehaviour
{
    [SerializeField] private GameObject fadeGameobject;
    private Image fadeImage;
    private void Awake()
    {
        if (!fadeGameobject.activeSelf)
        {
            fadeGameobject.SetActive(true);
        }

        fadeImage = fadeGameobject.GetComponent<Image>();
    }

    private void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
        LeanTween.alpha(fadeImage.rectTransform, 0f, 1f)
            .setEase(LeanTweenType.easeInOutQuad);
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        LeanTween.alpha(fadeImage.rectTransform, 1f, 0.8f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() => SceneManager.LoadScene(sceneName));
    }
}
