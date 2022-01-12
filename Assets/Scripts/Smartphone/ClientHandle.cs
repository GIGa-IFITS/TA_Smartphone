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
            NetworkUIManager.instance.ClientConnected(_msg);
        }

        //Send welcome received packet
        ClientSend.WelcomeReceived();
    }

    public static void PhoneSizeReceived(PacketNetwork _packet){
        float _screenWidth = _packet.ReadFloat();
        float _screenHeight = _packet.ReadFloat();

        Debug.Log($"Received phone size from server.");
        // for vr
        //VirtualSmartphone.instance.UpdatePhoneSize(_screenWidth, _screenHeight);
    }
    
    // receive texture from server
    public static void TextureReceived(PacketNetwork _packet){
        byte[] _receivedTexture2D = _packet.ReadBytes(_packet.UnreadLength());

        Debug.Log($"Received texture2D from server.");
        // for vr
        //VirtualSmartphone.instance.CopyTexture2DToRenderTexture(_receivedTexture2D);
    }

    // receive dashboard toggle data from server
    public static void ToggleReceived(PacketNetwork _packet){
        bool _toggleVal = _packet.ReadBool();

        Debug.Log($"Received dashboard toggle value from server.");
        // for vr
        //Manager.instance.Dashboard();
        //Manager.instance.DashboardToggle();
    }

    // receive command from server, for vr
    public static void CommandReceived(PacketNetwork _packet){
        string _command = _packet.ReadString();

        Debug.Log($"Received command: " +  _command + " from server.");
        if (_command == "name"){
            Debug.Log("Showing researcher name");
            //Manager.instance.getPenelitiAbjadITS();
        }
        else if (_command == "unit"){
            Debug.Log("Showing institution unit");
            //Manager.instance.getPenelitiFakultasITS();
        }
        else if (_command == "degree"){
            Debug.Log("Showing academic degree");
            //Manager.instance.getGelarPenelitiITS();
        }
        else if (_command == "keyword"){
            Debug.Log("Showing research keyword");
            //Manager.instance.getPublikasiFakultas();
        }
        else if (_command == "destroy"){
            Debug.Log("Destroying node, back to filter menu");
            //Manager.instance.flushNode();
        }
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
        // for vr
        // string _id = _packet.ReadString();
        // string _tagName = _packet.ReadString();

        // // update smartphone screen
        // VirtualSmartphone.instance.PreviousNode(_id, _tagName);
    }

    // receive request for texture message from server
    // public static void TextureRequested(PacketNetwork _packet){
    //     string _msg = _packet.ReadString();

    //     Debug.Log($"Message from server: {_msg}");

    //     //Send texture 
    //     ClientSend.SendTexture();
    // }

    // receive dashboard data from server
    // public static void DashboardDataReceived(PacketNetwork _packet){
    //     int journals = _packet.ReadInt();
    //     int conferences = _packet.ReadInt();
    //     int books = _packet.ReadInt();
    //     int thesis = _packet.ReadInt();
    //     int patents = _packet.ReadInt();
    //     int research = _packet.ReadInt();

    //     // copy data to smartphone screen
    // }
    
    
}
