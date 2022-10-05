using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform textPos;
    private Vector3 defaultPos;
    private void Awake()
    {
        textPos = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        defaultPos = textPos.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("down");
        textPos.position = new Vector3(defaultPos.x, defaultPos.y - 3);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        textPos.position = defaultPos;
    }
}
