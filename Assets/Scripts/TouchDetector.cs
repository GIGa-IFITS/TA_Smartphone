using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    private float startTime;
    public float SWIPE_THRESHOLD;
    private bool isContentInPhone = true;
    private List<TouchLocation> touches = new List<TouchLocation>();
    public float cooldownTime;
    public float timeLastChanged;
    private float distance;
    private float diffTime;
    private float speed;
    public bool isScrolling = false;

    void Update()
    {
        if(Input.touchCount >= 3){
            foreach (Touch touch in Input.touches){
                if (touch.phase == TouchPhase.Began)
                {
                    touches.Add(new TouchLocation(touch.fingerId, touch.position));
                }
                
                //Detects swipe after finger is released
                else if (touch.phase == TouchPhase.Ended)
                {
                    TouchLocation thisTouch = touches.Find(TouchLocation => TouchLocation.touchId == touch.fingerId);
                    if(thisTouch != null && Time.time - timeLastChanged > cooldownTime){
                        thisTouch.endPos = touch.position;
                        CheckMultiSwipe(thisTouch);
                    }
                    touches.Clear();
                }
            }
        }
        else if(Input.touchCount == 1){
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
                startTime = Time.time;
                isScrolling = false;
            }

            if(touch.phase == TouchPhase.Stationary){
                startPos = touch.position;
                if(isScrolling){
                    startTime = Time.time;
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                isScrolling = true;
                endPos = touch.position;
                distance = endPos.y - startPos.y;
                diffTime = Time.time - startTime;

                CalculateScrollSpeed(distance, diffTime);
                startPos = touch.position;
                startTime = Time.time;
            }
            
            if (touch.phase == TouchPhase.Ended)
            {
                endPos = touch.position;
                distance = endPos.y - startPos.y;
                diffTime = Time.time - startTime;

                CheckTouch(distance, diffTime);
            }
        }
    }

    private void CheckMultiSwipe(TouchLocation _touch)
    {
        if (_touch.endPos.y - _touch.startPos.y > SWIPE_THRESHOLD && isContentInPhone)//swipe up
        {
            Debug.Log("swipe up");
            ClientSend.SendSwipe("up");
            Handheld.Vibrate();
            isContentInPhone = false;
            timeLastChanged = Time.time;
        }
        else if (_touch.endPos.y - _touch.startPos.y < -SWIPE_THRESHOLD && !isContentInPhone)//swipe down
        {
            Debug.Log("swipe down");
            ClientSend.SendSwipe("down");
            Handheld.Vibrate();
            isContentInPhone = true;
            timeLastChanged = Time.time;  
        }
    }

    private void CalculateScrollSpeed(float _distance, float _diffTime){
        if(Mathf.Abs(_distance) > SWIPE_THRESHOLD){
            speed = (_distance / _diffTime);
            if(speed != Mathf.Infinity && speed != 0){
                Debug.Log("scroll with speed : " + speed);
                ClientSend.SendScrollSpeed(speed);
            }
        }
    }

    private void CheckTouch(float _distance, float _diffTime){
        if(isScrolling){
            CalculateScrollSpeed(_distance, _diffTime);
        }else{
            Debug.Log("touch!");
            ClientSend.SendCommand("touch");
        }
        isScrolling = false; 
    }
}
