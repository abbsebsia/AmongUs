using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTask : MonoBehaviour
{
    public List<SwipePoint> _swipePoints = new List<SwipePoint>();
    public float _countDownMax = 0.5f;
    private int _currentSwipePointIndex = 0;
    private float _countDown = 0.5f;

    private void Update() {
        _countDown -= Time.deltaTime;

        if (_currentSwipePointIndex !=0 && _countDown <= 0) {
            _currentSwipePointIndex = 0;
            Debug.Log("Error");
        }
    }

    public void SwipePointTrigger(SwipePoint swipePoint) {
        if (swipePoint == _swipePoints[_currentSwipePointIndex]) {
            _currentSwipePointIndex ++;
            _countDown = _countDownMax;
        }
        if (_currentSwipePointIndex >= swipePoints.Count) {
            _currentSwipePointIndex = 0;
            Debug.Log("Finished");

        }
    }
}
