using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonNode : MonoBehaviour
{
    [System.Serializable]
    public class HoverEvent : UnityEvent { }
    [SerializeField] HoverEvent OnHoverEvent;

    [System.Serializable]
    public class ClickEvent : UnityEvent { }
    [SerializeField] ClickEvent OnClickEvent;

    private void OnMouseOver()
    {
        OnHoverEvent.Invoke();
    }
    private void OnMouseDown()
    {
        OnClickEvent.Invoke();
    }
}
