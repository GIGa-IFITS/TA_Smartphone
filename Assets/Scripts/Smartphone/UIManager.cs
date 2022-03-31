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
    [SerializeField] private GameObject dashboardMenu;
    [SerializeField] private GameObject searchMenu;
    [SerializeField] private GameObject connectButton;
    [SerializeField] private GameObject dashboardPanel;
    [SerializeField] private GameObject dashboardErrorPanel;
    [SerializeField] private GameObject dashboardLoading;
    private TextMeshProUGUI connectButtonText;
    private TextMeshProUGUI ipInputText;
    [SerializeField] private TextMeshProUGUI detailText;
    private bool connectSuccess;

    // connect to server
    RequestHandler requestPeneliti = new RequestHandler();
    private string URL = "https://127.0.0.5:5000";
    // dashboard text
    private TextMeshProUGUI journals;
    private TextMeshProUGUI conferences;
    private TextMeshProUGUI books;
    private TextMeshProUGUI thesis;
    private TextMeshProUGUI patents;
    private TextMeshProUGUI research;
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

        journals = dashboardMenu.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        conferences = dashboardMenu.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        books = dashboardMenu.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
        thesis = dashboardMenu.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(2).GetChild(3).GetComponent<TextMeshProUGUI>();
        patents = dashboardMenu.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(2).GetChild(4).GetComponent<TextMeshProUGUI>();
        research = dashboardMenu.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(2).GetChild(5).GetComponent<TextMeshProUGUI>();
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
        ChangeMenuScreen("dashboardMenu");
    }

     public void ChangeMenuScreen(string _pageType){
        switch (_pageType) {
            case "dashboardMenu":
                dashboardLoading.SetActive(true);
                dashboardPanel.SetActive(false);
                dashboardErrorPanel.SetActive(false);
                dashboardMenu.SetActive(true);
                break;

            case "dashboardData":
                // get dashboard data
                requestPeneliti.URL = URL;
                StartCoroutine(requestPeneliti.RequestData((result) =>
                {
                    // mengambil jumlah jurnal, conference, books, thesis, paten dan research yang ada
                    hasilPublikasiITS(result);
                    
                }, (error) => {
                    if (error != "")
                    {

                    }
                }));
                dashboardLoading.SetActive(false);
                dashboardPanel.SetActive(true);   
                break;

            case "dashboardError":
                dashboardLoading.SetActive(false);
                dashboardPanel.SetActive(false);
                dashboardErrorPanel.SetActive(true);
                break;
    
            case "searchMenu":
                dashboardMenu.SetActive(false);
                searchMenu.SetActive(true);
                break;
        }
    }


    private void hasilPublikasiITS(RawData rawdata)
    {
        journals.text = rawdata.data[0].dashboard_data[0].hasil_publikasi[0].journals.ToString();
        conferences.text = rawdata.data[0].dashboard_data[0].hasil_publikasi[1].conferences.ToString();
        books.text = rawdata.data[0].dashboard_data[0].hasil_publikasi[2].books.ToString();
        thesis.text = rawdata.data[0].dashboard_data[0].hasil_publikasi[3].thesis.ToString();
        patents.text = rawdata.data[0].dashboard_data[0].hasil_publikasi[4].paten.ToString();
        research.text = rawdata.data[0].dashboard_data[0].hasil_publikasi[5].research.ToString();
    }

    public void Disconnected(){
        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive(false);
        }
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
