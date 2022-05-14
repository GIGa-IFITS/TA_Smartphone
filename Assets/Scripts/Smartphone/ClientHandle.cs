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

    public static void PhoneStatusReceived(PacketNetwork _packet){
        //Manager.instance.SetVirtualSmartphoneCanvasActive();
    }

    // receive command from server
    public static void CommandReceived(PacketNetwork _packet){
        // string _command = _packet.ReadString();

        // Debug.Log($"Received command: " +  _command + " from server.");
        // if (_command == "name"){
        //     Debug.Log("Showing researcher name");
        //     Manager.instance.getPenelitiAbjadITS();
        // }
        // else if (_command == "unit"){
        //     Debug.Log("Showing institution unit");
        //     Manager.instance.getPenelitiFakultasITS();
        // }
        // else if (_command == "degree"){
        //     Debug.Log("Showing academic degree");
        //     Manager.instance.getGelarPenelitiITS();
        // }
        // else if (_command == "keyword"){
        //     Debug.Log("Showing research keyword");
        //     Manager.instance.getPublikasiFakultas();
        // }
        // else if (_command == "destroy"){
        //     Debug.Log("Destroying node, back to filter menu");
        //     Manager.instance.flushNode();
        // }

        // // for canvas 
        // if(_command != "destroy"){
        //     VirtualSmartphoneCanvas.instance.UpdateScreenFilter(_command);
        // }
    }

    // receive filter summary
    public static void FilterSummaryReceived(PacketNetwork _packet){
        string _name = _packet.ReadString();
        int _total = _packet.ReadInt();
        string _tag = _packet.ReadString();
        string _nodeId = _packet.ReadString();
        string _filterName = _packet.ReadString();

        // update smartphone screen
        FilterManager.instance.ShowFilterSummary(_name, _total, _tag, _nodeId, _filterName);
    }

    public static void ResearcherIdReceived(PacketNetwork _packet){
        string _id = _packet.ReadString();
        string _filterName = _packet.ReadString();

        // update smartphone screen
        FilterManager.instance.ShowResearcherDetail(_id, _filterName);
    }

    public static void NodeRequestReceived(PacketNetwork _packet){
        // string _nodeId = _packet.ReadString();
        // string _nodeId2 = _packet.ReadString();
        // string _tagName = _packet.ReadString();

        // // update node
        // Manager.instance.GetNode(_nodeId, _nodeId2, _tagName);
    }

    public static void ErrorMessageReceived(PacketNetwork _packet){
        string _errorMsg = _packet.ReadString();

        // update smartphone screen
        FilterManager.instance.ShowErrorScreen(_errorMsg);
    }

    public static void OrientationReceived(PacketNetwork _packet){
        // bool _isUp = _packet.ReadBool();
        // VirtualSmartphone.instance.SetDeviceOrientation(_isUp);
    }

    public static void NodeSizeReceived(PacketNetwork _packet){
        // float _nodeSize = _packet.ReadFloat();

        // Manager.instance.resizeNode(_nodeSize);
        // VirtualSmartphoneCanvas.instance.UpdateScreenSettingsNode(_nodeSize);
    }

    public static void PageTypeReceived(PacketNetwork _packet){
        // string _pageType = _packet.ReadString();
        // Debug.Log("receive page type " + _pageType);
        // UIManager.instance.ChangeMenuScreen(_pageType);
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
}
