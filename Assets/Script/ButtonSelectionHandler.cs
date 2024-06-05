using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectionHandler : MonoBehaviour, ISelectHandler
{
    public delegate void ButtonSelected(Button button);
    public event ButtonSelected OnButtonSelected;

    public void OnSelect(BaseEventData eventData)
    {
        Button button = GetComponent<Button>();
        if (button != null && OnButtonSelected != null)
        {
            OnButtonSelected(button);
        }
    }
}
