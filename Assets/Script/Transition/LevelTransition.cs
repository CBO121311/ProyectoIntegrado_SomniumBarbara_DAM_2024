using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    
    [SerializeField] private GameObject levelBackGround;
    [SerializeField] private GameObject blackBackGround;
    [SerializeField] private GameObject symbol;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject alarmClock;

    void Start()
    {


        /*LeanTween.moveX(backGround.GetComponent<RectTransform>(), 0, 1.5f)
        .setEase(LeanTweenType.easeOutBounce);*/

        if (levelBackGround.activeSelf && symbol.activeSelf)
        {
            Time.timeScale = 0;
            LeanTween.moveY(symbol.GetComponent<RectTransform>(), 0, 1.5f)
               .setEase(LeanTweenType.easeInOutCubic).setIgnoreTimeScale(true);

            LeanTween.alpha(levelBackGround.GetComponent<RectTransform>(), 0f, 0.5f).setDelay(2.5f)
                .setIgnoreTimeScale(true)
                    .setOnComplete(() =>
                    {
                        Time.timeScale = 1;
                    });
        }
    }


    public void OpenPauseMenu()
    {
        LeanTween.alpha(blackBackGround.GetComponent<RectTransform>(), 0.5f, 0.2f)
    .setIgnoreTimeScale(true);
        LeanTween.scale(pauseMenu.GetComponent<RectTransform>(), new Vector3(2, 2, 1), 0.5f)
            .setIgnoreTimeScale(true)
            .setEase(LeanTweenType.easeOutBack);
    }

    public void ExitPauseMenu()
    {
        LeanTween.scale(pauseMenu.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 0.5f);
        LeanTween.alpha(blackBackGround.GetComponent<RectTransform>(), 0, 0.5f);
    }

    // Realiza la animación de la alarma
    public void ActivateAlarmClock()
    {
    
    Vector3 initialPosition = alarmClock.GetComponent<RectTransform>().anchoredPosition;

    LeanTween.moveX(alarmClock.GetComponent<RectTransform>(), initialPosition.x -30f, 0.3f).setEaseInOutQuad().setLoopCount(5);

    LeanTween.moveY(alarmClock.GetComponent<RectTransform>(), initialPosition.y -20f, 0.2f).setEaseInOutQuad().setLoopCount(8)
        .setOnComplete(() =>
        {
            LeanTween.move(alarmClock.GetComponent<RectTransform>(), initialPosition, 0.3f).setEaseInOutQuad();
        });
    }
}
