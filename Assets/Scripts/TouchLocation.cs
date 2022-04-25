using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLocation
{
    public int touchId;
    public Vector2 startPos;
    public Vector2 endPos;

    public TouchLocation(int _touchId, Vector2 _startPos){
        touchId = _touchId;
        startPos = _startPos;
    }
}
