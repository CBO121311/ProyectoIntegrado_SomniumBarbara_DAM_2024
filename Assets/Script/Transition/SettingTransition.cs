using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingTransition : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;


    //Método que realiza las animaciones al completar un nivel
    public void OpenSettingLevel()
    {
        settingPanel.SetActive(true);
        LeanTween.alphaCanvas(settingPanel.GetComponent<CanvasGroup>(), 1f, 0.6f)
            .setEase(LeanTweenType.easeOutQuad);
    }

    public void CloseSettingLevel()
    {
        LeanTween.alphaCanvas(settingPanel.GetComponent<CanvasGroup>(), 0f, 0.6f)
            .setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
            settingPanel.SetActive(false)
            );
    }
}
