using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Defines right-click support for UI elements.
/// </summary>
public class RightClick : MonoBehaviour, IPointerClickHandler
{
    public event Action onRightClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick?.Invoke();
        }
    }

    /// <summary>
    /// Clear all events that are triggered on right click.
    /// </summary>
    public void ClearAction()
    {
        onRightClick = null;
    }
}