using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(PacketNetwork _packet){
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;

        // if smartphone
        if(Client.instance.myId == 2){
            UIManager.instance.ClientConnected(_msg);
        }

        //Send welcome received packet
        ClientSend.WelcomeReceived();
    }

    // receive vibrate command from server
    public static void VibrateReceived(PacketNetwork _packet){
        string _msg = _packet.ReadString();

        Handheld.Vibrate();

        if(_msg == "up"){
            TouchDetector.instance.SetIsContentInPhone(false);
        }else if(_msg == "down"){
            TouchDetector.instance.SetIsContentInPhone(true);
        }
    }

    public static void SwipeReceived(PacketNetwork _packet){
        // string _swipeType = _packet.ReadString();
        // Debug.Log("receive swipe type " + _swipeType);
        // Manager.instance.SetScreenMode(_swipeType);
    }

    public static void ScrollSpeedReceived(PacketNetwork _packet){
        // float _scrollSpeed = _packet.ReadFloat();
        // Debug.Log("receive scroll speed " + _scrollSpeed);
        // ScreenManager.instance.SetScroll(_scrollSpeed);
    }

     public static void RotationReceived(PacketNetwork _packet){
        // float _x = _packet.ReadFloat();
        // float _y = _packet.ReadFloat();
        // float _z = _packet.ReadFloat();
        // float _w = _packet.ReadFloat();
        // SmartphoneGyro.instance.SetPhoneRotation(_x, _y, _z, _w);
    }    
}
