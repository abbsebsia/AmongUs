using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class SwipePoint : MonoBehaviour {
   
   private SwipeTask _swipeTask;
   
   private void Awake() {
      _swipeTask = GetComponentInParent<SwipeTask>();
   }
   private void OnTriggerEnter2D(Collider2D other) {
      _swipeTask.SwipePointTrigger(this);
   }
}
