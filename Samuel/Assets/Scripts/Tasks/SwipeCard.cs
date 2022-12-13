using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeCard : MonoBehaviour {
   private Canvas _canvas;
   
   private void Awake() {
      _canvas = GetComponentInParent<Canvas>();
   }
   
}
