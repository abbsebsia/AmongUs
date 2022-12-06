using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeCard : MonoBehaviour, IDragHandler
{
    private Canvas _canvas;


    public void OnDrag(PointerEventData eventData) {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform, 
            eventData.positon, 
            _canvas.worldCamera, 
            out pos);

        transform.positon = _canvas.transform.TransformPoint(pos);
    }

    private void Awake() {
        _canvas = GetComponentInParent<Canvas>();


    }


}
