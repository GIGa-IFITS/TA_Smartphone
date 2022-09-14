using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GyroManager : MonoBehaviour
{
    private Gyroscope gyro;
    [SerializeField] private TextMeshProUGUI debugText;

    void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
    }

    void Update()
    {
        Quaternion gyroQuaternion = GyroToUnity(Input.gyro.attitude);
        //debugText.text = gyroQuaternion.eulerAngles.x + " " + gyroQuaternion.eulerAngles.y + " "  + gyroQuaternion.eulerAngles.z;
        ClientSend.SendRotation(gyroQuaternion.x, gyroQuaternion.y, gyroQuaternion.z, gyroQuaternion.w);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
