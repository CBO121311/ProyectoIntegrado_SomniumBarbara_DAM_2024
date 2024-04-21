using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("First Selected Button")]
    [SerializeField] private Selectable firstSelected;

    //Para sobreescribirlo
    protected virtual void OnEnable()
    {
        SetFirstSelected(firstSelected);
    }

    public void SetFirstSelected(Selectable firstSelectedButton)
    {
        firstSelectedButton.Select();
    }
}
