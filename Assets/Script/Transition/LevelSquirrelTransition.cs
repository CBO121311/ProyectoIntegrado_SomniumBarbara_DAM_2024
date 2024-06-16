using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSquirrelTransition : MonoBehaviour
{
    [Header("Opening Level")]
    [SerializeField] private GameObject backgroundOpeningLevel;
    [SerializeField] private GameObject bookImage;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseBackground; 
    [SerializeField] private GameObject pauseMenu;


    [Header("Complete Level")]
    [SerializeField] private GameObject passTransition;
    [SerializeField] private GameObject scoreGroup;
    [SerializeField] private GameObject actionCompleteText;

    [Header("Other")]
    [SerializeField] private GameObject alarmTimer;
    [SerializeField] private GameObject passMessage;
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

    void Start()
    {
        FadeIn();
    }


    public void FadeIn()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
        LeanTween.alpha(fadeImage.rectTransform, 0f, 0.8f)
            .setEase(LeanTweenType.easeInOutQuad).setOnComplete(()=>
            {
                AudioManager.Instance.PlayMusic(2);

                if (backgroundOpeningLevel.activeSelf && bookImage.activeSelf)
                {
                    Time.timeScale = 0;
                    LeanTween.moveY(bookImage.GetComponent<RectTransform>(), 0, 1.5f)
                       .setEase(LeanTweenType.easeInOutCubic).setIgnoreTimeScale(true);

                    LeanTween.alpha(backgroundOpeningLevel.GetComponent<RectTransform>(), 0f, 0.5f).setDelay(3f)
                        .setIgnoreTimeScale(true)
                            .setOnComplete(() =>
                            {
                                Time.timeScale = 1;
                            });
                }
            });
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        LeanTween.alpha(fadeImage.rectTransform, 1f, 1f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() => SceneManager.LoadScene(sceneName));
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        LeanTween.alpha(pauseBackground.GetComponent<RectTransform>(), 0.5f, 0.2f)
        .setIgnoreTimeScale(true);

        LeanTween.scale(pauseMenu.GetComponent<RectTransform>(), new Vector3(2, 2, 1), 0.5f)
            .setIgnoreTimeScale(true)
            .setEase(LeanTweenType.easeOutBack);
    }

    public void ClosePauseMenu()
    {
        LeanTween.scale(pauseMenu.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 0.5f)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
            });

        LeanTween.alpha(pauseBackground.GetComponent<RectTransform>(), 0, 0.5f);
    }

    // Realiza la animación de la alarma
    public void ActivateAlarmClock()
    {

        Vector3 initialPosition = alarmTimer.GetComponent<RectTransform>().anchoredPosition;

        LeanTween.moveX(alarmTimer.GetComponent<RectTransform>(), initialPosition.x - 30f, 0.3f).setEaseInOutQuad().setLoopCount(5);

        LeanTween.moveY(alarmTimer.GetComponent<RectTransform>(), initialPosition.y - 20f, 0.2f).setEaseInOutQuad().setLoopCount(8)
            .setOnComplete(() =>
            {
                LeanTween.move(alarmTimer.GetComponent<RectTransform>(), initialPosition, 0.3f).setEaseInOutQuad();
            });
    }

    //Método que realiza las animaciones de passMessage
    public void RotatePassMessage()
    {
        float rotationAmount = 5f;

        passMessage.SetActive(true);

        LeanTween.alphaCanvas(passMessage.GetComponent<CanvasGroup>(), 1f, 1f);

        LeanTween.rotateZ(passMessage.gameObject, rotationAmount, 1f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setLoopPingPong();

        LeanTween.alphaCanvas(passMessage.GetComponent<CanvasGroup>(), 0f, 1f)
            .setDelay(4f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                passMessage.SetActive(false);
            });
    }

    //Método que realiza las animaciones al completar un nivel
    public void AnimateLevelComplete()
    {
        
        LeanTween.moveX(passTransition.GetComponent<RectTransform>(), 0, 1f)
            .setEase(LeanTweenType.easeOutBack)
            .setOnComplete(ShowScoreGroup)
            .setIgnoreTimeScale(true);

        void ShowScoreGroup()
        {
            if (scoreGroup != null)
            {
                scoreGroup.SetActive(true);

                LeanTween.alphaCanvas(scoreGroup.GetComponent<CanvasGroup>(), 1f, 1f)
                    .setEase(LeanTweenType.easeOutQuad)
                    .setDelay(1f)
                    .setOnComplete(ShowActionCompleteText)
                    .setIgnoreTimeScale(true);
            }
        }

        void ShowActionCompleteText()
        {
            if (actionCompleteText != null)
            {
                LeanTween.delayedCall(1f, () =>
            {
                actionCompleteText.SetActive(true);

                TextMeshProUGUI textMesh = actionCompleteText.GetComponent<TextMeshProUGUI>();
                Color textColor = textMesh.color;
                LeanTween.value(textColor.a, 1, 0.5f)
                    .setOnUpdate((float alpha) =>
                    {
                        textColor.a = alpha;
                        textMesh.color = textColor;
                    }).setIgnoreTimeScale(true);
            }).setIgnoreTimeScale(true);
            }
        }
    }
}
