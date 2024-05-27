using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_SelectionLevel : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject inventoryLevel;
    Vector3 initPosPauseMenu, initPosInventoryLevel;

    private void Start()
    {
        initPosPauseMenu = pauseMenu.GetComponent<RectTransform>().anchoredPosition;
        initPosInventoryLevel = inventoryLevel.GetComponent<RectTransform>().anchoredPosition;
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
            .setEaseInOutQuad().setOnComplete(() => {
                pauseMenu.SetActive(false);
            });
    }

    //Animación al abrir el inventario
    public void OpenInventory()
    {
        Time.timeScale = 0;
        LeanTween.moveY(inventoryLevel.GetComponent<RectTransform>(), 0, 0.5f)
            .setEaseOutQuint().setIgnoreTimeScale(true);
    }

    public void CloseInventory()
    {
        LeanTween.moveY(inventoryLevel.GetComponent<RectTransform>(), initPosInventoryLevel.y, 0.5f)
            .setEaseOutQuint().setIgnoreTimeScale(true)
        .setOnComplete(() => {
            Time.timeScale = 1;
        }) ;
    }
}
