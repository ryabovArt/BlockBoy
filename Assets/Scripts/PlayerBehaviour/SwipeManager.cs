using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public static SwipeManager instance;

    public float inputFloatX;
    public float inputFloatY;

    private void Awake()
    {
        if (SwipeManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        SwipeManager.instance = this;
    }

    private void OnDestroy()
    {
        SwipeManager.instance = null;
    }

    /// <summary>
    /// Дейстаия при свайпе по экрану
    /// </summary>
    /// <param name="eventData"> событие свайпа </param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y)) // если движение по Х больше чем по Y
        {
            if (eventData.delta.x > 0) // если ведем вправо
            {
                Debug.Log("ведем вправо");
                inputFloatX = 1f;
            }
            else // если ведем влево
            {
                Debug.Log("ведем влево");
                inputFloatX = -1f;
            }
        }
        else
        {
            if (eventData.delta.y > 0) // если ведем вверх
            {
                Debug.Log("ведем вверх");
                inputFloatY = 1f;
            }
            else // если ведем вниз
            {
                Debug.Log("ведем вниз");
                inputFloatY = -1f;
            }
        }
    }

    public void OnDrag(PointerEventData eventData) { }
}
