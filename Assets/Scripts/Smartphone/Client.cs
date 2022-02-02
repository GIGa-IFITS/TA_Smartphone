using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1"; //server ip
    public int port = 6000; //server port
    public int myId = 0;
    public TCP tcp;
    private bool isConnected = false;
    private delegate void PacketHandler(PacketNetwork _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake() {
        if (instance == null){
            Debug.Log("client instantiated");
            instance = this;
        }    
        else if(instance != this){
            Debug.Log("Instance already exists, destroying object");
            Destroy(this);
        }
        tcp = new TCP();
    }

    private void OnApplicationQuit(){
        Disconnect();
    }

    public void ConnectToServer(){
        InitializeClientData();
        isConnected = true;
        tcp.Connect();
    }

    public class TCP{
        public TcpClient socket;
        private NetworkStream stream;
        private PacketNetwork receivedData;
        private byte[] receiveBuffer;
        public void Connect(){
            socket = new TcpClient{
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            try{
                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            }
            catch(Exception _ex){
                Debug.Log(_ex.ToString());
            }
        }
        private void ConnectCallback(IAsyncResult _result){
            socket.EndConnect(_result);
            if(!socket.Connected){
                return;
            }

            stream = socket.GetStream();

            receivedData = new PacketNetwork();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        public void SendData(PacketNetwork _packet){
            try{
                if(socket != null){
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch(Exception _ex){
                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result){
            try{
                int _byteLength = stream.EndRead(_result);
                if(_byteLength <= 0){
                    instance.Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                // handle data
                receivedData.Reset(HandleData(_data));

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch{
                Disconnect();
            }
        }

        private bool HandleData(byte[] _data){
            int _packetLength = 0;
            
            receivedData.SetBytes(_data);

            // int = 4 bytes, first int is the length of the packet
            if(receivedData.UnreadLength() >= 4){
                _packetLength = receivedData.ReadInt();
                if(_packetLength <= 0){
                    return true; // reset received data
                }
            }

            // if packet still have continuation
            while(_packetLength > 0 && _packetLength <= receivedData.UnreadLength()){
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() => {
                    using (PacketNetwork _packet = new PacketNetwork(_packetBytes)){
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                _packetLength = 0;
                if(receivedData.UnreadLength() >= 4){
                    _packetLength = receivedData.ReadInt();
                    if(_packetLength <= 0){
                        return true; // reset received data
                    }
                }
            }

            if(_packetLength <= 1){
                return true;
            }

            return false;
        }

        private void Disconnect(){
            instance.Disconnect();

            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    // for received packets from server
    private void InitializeClientData(){
        if(packetHandlers != null){
            Debug.Log("Packet already initialized");
            return;
        }
        
        packetHandlers = new Dictionary<int, PacketHandler>(){
            { (int)ServerPackets.welcome, ClientHandle.Welcome },
            { (int)ServerPackets.sendPhoneSize, ClientHandle.PhoneSizeReceived },
            { (int)ServerPackets.sendTexture, ClientHandle.TextureReceived }, 
            { (int)ServerPackets.sendDashboardToggle, ClientHandle.ToggleReceived },
            { (int)ServerPackets.sendCommand, ClientHandle.CommandReceived },
            { (int)ServerPackets.sendFilterSummary, ClientHandle.FilterSummaryReceived },
            { (int)ServerPackets.sendResearcherId, ClientHandle.ResearcherIdReceived },
            { (int)ServerPackets.sendNodeRequest, ClientHandle.NodeRequestReceived },
            { (int)ServerPackets.sendErrorMessage, ClientHandle.ErrorMessageReceived }
        };
        Debug.Log("initialized packets");
    }

    private void Disconnect(){
        if(isConnected){
            isConnected = false;
            tcp.socket.Close();
        }
    }
}
