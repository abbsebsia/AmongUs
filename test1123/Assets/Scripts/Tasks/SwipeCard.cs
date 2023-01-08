using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class SwipeCard : MonoBehaviour, IDragHandler {
   private Canvas _canvas;
   public Transform card;
   
   private void Awake() {
      _canvas = GetComponentInParent<Canvas>();
   }
   
   public void OnDrag(PointerEventData eventData) {
      Vector2 pos;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(
               _canvas.transform as RectTransform,
               Input.mousePosition,
               _canvas.worldCamera,
               out pos);
      transform.position = _canvas.transform.TransformPoint(pos);
   }
}