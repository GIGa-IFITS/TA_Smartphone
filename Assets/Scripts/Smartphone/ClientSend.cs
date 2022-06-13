using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(PacketNetwork _packet){
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }


    #region Packets

    public static void WelcomeReceived(){
        using (PacketNetwork _packet = new PacketNetwork((int)ClientPackets.welcomeReceived)){
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void SendTouch(string _touch){
        using(PacketNetwork _packet = new PacketNetwork((int)ClientPackets.sendTouch)){
            _packet.Write(Client.instance.myId);
            _packet.Write(_touch);

            SendTCPData(_packet);
        }
    }

    public static void SendSwipe(string _swipeType){
        using(PacketNetwork _packet = new PacketNetwork((int)ClientPackets.sendSwipe)){
            _packet.Write(Client.instance.myId);
            _packet.Write(_swipeType);

            SendTCPData(_packet);
        }
    }

    public static void SendScrollSpeed(float _speed){
        using(PacketNetwork _packet = new PacketNetwork((int)ClientPackets.sendScrollSpeed)){
            _packet.Write(Client.instance.myId);
            _packet.Write(_speed);

            SendTCPData(_packet);
        }
    }

    public static void SendRotation(float _x, float _y, float _z, float _w){
        using(PacketNetwork _packet = new PacketNetwork((int)ClientPackets.sendRotation)){
            _packet.Write(Client.instance.myId);
            _packet.Write(_x);
            _packet.Write(_y);
            _packet.Write(_z);
            _packet.Write(_w);

            SendTCPData(_packet);
        }
    }

    #endregion
}
