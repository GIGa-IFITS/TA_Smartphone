using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour
{
    private Gyroscope gyro;
    private Quaternion correctionQuaternion = Quaternion.Euler(90, 90, 0);
    private Quaternion phoneRotation;

    void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
    }

    void Update()
    {
        Quaternion gyroQuaternion = GyroToUnity(Input.gyro.attitude);
        phoneRotation = correctionQuaternion * gyroQuaternion;
        ClientSend.SendRotation(phoneRotation.x, phoneRotation.y, phoneRotation.z, phoneRotation.w);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
