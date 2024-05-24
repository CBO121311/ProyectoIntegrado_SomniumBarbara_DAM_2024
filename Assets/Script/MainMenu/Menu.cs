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
        if (firstSelected != null)
        {
            SetFirstSelected(firstSelected);
        }
    }

    public void SetFirstSelected(Selectable firstSelectedButton)
    {
        firstSelectedButton.Select();
    }
}
