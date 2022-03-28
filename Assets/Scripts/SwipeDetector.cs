using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public float SWIPE_THRESHOLD = 20f;

    void Update()
    {
        if(Input.touchCount >= 3){
            foreach (Touch touch in Input.touches){
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUp = touch.position;
                    fingerDown = touch.position;
                }

                //Detects swipe after finger is released
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerUp = touch.position;
                    checkSwipe();
                }
            }
        }
    }

    private void checkSwipe()
    {
        if (Mathf.Abs(fingerUp.y - fingerDown.y) > SWIPE_THRESHOLD)
        {
            if (fingerUp.y - fingerDown.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerUp.y - fingerDown.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }
    }

    private void OnSwipeUp()
    {
        Debug.Log("Swipe UP");
    }

    private void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
    }
}
