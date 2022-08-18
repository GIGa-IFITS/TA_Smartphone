using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
/* SCRIPT UNTUK APLIKASI DI SMARTPHONE */
public class UIManager : MonoBehaviour {
    public static UIManager instance;
    [SerializeField] private TMP_InputField ipField;
    [SerializeField] private GameObject connectMenu;
    [SerializeField] private GameObject searchingMenu;
    [SerializeField] private GameObject connectButton;
    private TextMeshProUGUI connectButtonText;
    private TextMeshProUGUI ipInputText;
    [SerializeField] private TextMeshProUGUI detailText;
    private bool connectSuccess;

    // connect to server
    RequestHandler requestPeneliti = new RequestHandler();
    private string URL = "https://127.0.0.5:5000";
    // dashboard text
    public TextMeshProUGUI debuggingText;
    private bool isDisconnectButtonPressed = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start ()
    {
        ipField.onEndEdit.AddListener(SubmitIP);
        connectSuccess = false;
        connectButtonText = connectButton.GetComponentInChildren<TextMeshProUGUI>();
        ipInputText = ipField.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        Client.instance.ip = ipInputText.text;
    }

    // to connect to VR
    private void SubmitIP(string _ip)
    {
        Client.instance.ip = _ip;
    }

    // to connect to database
    private void SetURL(string _url){
        URL = _url;
    }

    public void ConnectToServer()
    {
        detailText.gameObject.SetActive(true);
        detailText.text = "Connecting...";
        detailText.color = new Color32(50, 59, 89, 255);
        connectButtonText.text = "Connect";
        connectButtonText.color = new Color32(142, 144, 149, 255);
        connectButton.GetComponent<Button>().interactable = false;
        ipField.interactable = false;
        ipInputText.color = new Color32(142, 144, 149, 255);
        
        StartCoroutine(WaitForNSeconds(1f));
        Client.instance.ConnectToServer();
    }

    IEnumerator WaitForNSeconds(float n)
    {
        yield return new WaitForSeconds(n);
        if(!connectSuccess){
            detailText.text = "Fail to connect";
            detailText.color = new Color32(231, 65, 65, 255);
            connectButtonText.text = "Try Again";
            connectButtonText.color = new Color32(255, 255, 255, 255);
            connectButton.GetComponent<Button>().interactable = true;
            ipField.interactable = true;
            ipInputText.color = new Color32(50, 59, 89, 255);
        } 
    }

    public void ClientConnected(string _url){
        connectSuccess = true;

        // set database url
        SetURL(_url);

        connectMenu.SetActive(false);
        searchingMenu.SetActive(true);
    }

    public void Disconnected(){
        searchingMenu.SetActive(false);
        connectMenu.SetActive(true);
        if(!isDisconnectButtonPressed){
            detailText.text = "Connection lost";
            detailText.color = new Color32(231, 65, 65, 255);
            connectButtonText.text = "Try Again";
            connectButtonText.color = new Color32(255, 255, 255, 255);
            ipInputText.color = new Color32(50, 59, 89, 255);
            
        }else{
            detailText.gameObject.SetActive(false);
            connectButtonText.text = "Connect";
            connectButtonText.color = new Color32(255, 255, 255, 255);
            ipInputText.color = new Color32(50, 59, 89, 255);
        }
        connectButton.GetComponent<Button>().interactable = true;
        ipField.interactable = true;
        isDisconnectButtonPressed = false;
    }
}
