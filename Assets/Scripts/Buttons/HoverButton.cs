using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject objectToShow;

    void Start()
    {
        // Hide the object on startup
        objectToShow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show the object when the mouse hovers over the button
        objectToShow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the object when the mouse stops hovering over the button
        objectToShow.SetActive(false);
    }
}
