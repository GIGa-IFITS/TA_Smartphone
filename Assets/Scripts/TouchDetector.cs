using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public float SWIPE_THRESHOLD = 20f;
    private int swipeUpCount;
    private int swipeDownCount;
    private bool isContentInPhone = true;
    [SerializeField] private bool isTouching = false;
    private Touch currTouch;

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
        else if(Input.touchCount == 1){
            if(Input.GetTouch(0).phase == TouchPhase.Ended){
                Debug.Log("touch!");
                ClientSend.SendCommand("touch");
            }
        }
    }

    private void checkSwipe()
    {
        if (Mathf.Abs(fingerUp.y - fingerDown.y) > SWIPE_THRESHOLD)
        {
            if (fingerUp.y - fingerDown.y > 0)//up swipe
            {
                swipeUpCount++;
                if(isContentInPhone && swipeUpCount >= 3){
                    Debug.Log("swipe up");
                    ClientSend.SendSwipe("up");
                    swipeUpCount = 0;
                    Handheld.Vibrate();
                    isContentInPhone = false;
                }
            }
            else if (fingerUp.y - fingerDown.y < 0)//Down swipe
            {
                swipeDownCount++;
                if(!isContentInPhone && swipeDownCount >= 3){
                    Debug.Log("swipe down");
                    ClientSend.SendSwipe("down");
                    swipeDownCount = 0;
                    Handheld.Vibrate();
                    isContentInPhone = true;
                }    
            }
            fingerUp = fingerDown;
        }
    }
}
