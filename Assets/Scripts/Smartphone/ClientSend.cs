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

    public static void SendCommand(string _command){
        using(PacketNetwork _packet = new PacketNetwork((int)ClientPackets.sendCommand)){
            _packet.Write(Client.instance.myId);
            _packet.Write(_command);

            SendTCPData(_packet);
        }
    }

    public static void SendFilterSummary(string _name, int _total, string _tag, string _nodeId, string _filterName){
        using(PacketNetwork _packet = new PacketNetwork((int)ClientPackets.sendFilterSummary)){
            _packet.Write(Client.instance.myId);
            _packet.Write(_name);
            _packet.Write(_total);
            _packet.Write(_tag);
            _packet.Write(_nodeId);
            _packet.Write(_filterName);

            SendTCPData(_packet);
        }
    }

    public static void SendResearcherId(string _id, string _filterName){
        using(PacketNetwork _packet = new PacketNetwork((int)ClientPackets.sendResearcherId)){
            _packet.Write(Client.instance.myId);
            _packet.Write(_id);
            _packet.Write(_filterName);

            SendTCPData(_packet);
        }
    }

    public static void SendNodeRequest(string _nodeId, string _nodeId2, string _tagName){
        using(PacketNetwork _packet = new PacketNetwork((int)ClientPackets.sendNodeRequest)){
            _packet.Write(Client.instance.myId);
            _packet.Write(_nodeId);
            _packet.Write(_nodeId2);
            _packet.Write(_tagName);

            SendTCPData(_packet);
        }
    }

    public static void SendErrorMessage(string _errorMsg){
        using(PacketNetwork _packet = new PacketNetwork((int)ClientPackets.sendErrorMessage)){
            _packet.Write(Client.instance.myId);
            _packet.Write(_errorMsg);

            SendTCPData(_packet);
        }
    }

    public static void SendOrientation(bool _isUp){
        using(PacketNetwork _packet = new PacketNetwork((int)ClientPackets.sendOrientation)){
            _packet.Write(Client.instance.myId);
            _packet.Write(_isUp);

            SendTCPData(_packet);
        }
    }

    public static void SendNodeSize(float _nodeSize){
        using(PacketNetwork _packet = new PacketNetwork((int)ClientPackets.sendNodeSize)){
            _packet.Write(Client.instance.myId);
            _packet.Write(_nodeSize);

            SendTCPData(_packet);
        }
    }

    public static void SendPageType(string _pageType){
        using(PacketNetwork _packet = new PacketNetwork((int)ClientPackets.sendPageType)){
            _packet.Write(Client.instance.myId);
            _packet.Write(_pageType);

            SendTCPData(_packet);
        }
    }

    #endregion
}
