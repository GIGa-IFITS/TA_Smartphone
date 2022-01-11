using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class NetworkUIManager : MonoBehaviour {
    public static NetworkUIManager instance;
    [SerializeField] private TMP_InputField ipField;
    [SerializeField] private GameObject connectMenu;
    [SerializeField] private GameObject dashboardMenu;
    [SerializeField] private GameObject filterMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject connectButton;
    [SerializeField] private GameObject dashboardPanel;
    [SerializeField] private GameObject dashboardErrorPanel;
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private GameObject detailErrorPanel;
    private TextMeshProUGUI connectButtonText;
    [SerializeField] private GameObject failText;
    [SerializeField] private Camera renderCamera;
    private Texture2D textureToSend2D;
    private bool connectSuccess;

    // connect to server
    RequestHandler requestPeneliti = new RequestHandler();
    private string URL = "https://127.0.0.5:5000";
    // dashboard text
    TextMeshProUGUI journals;
    TextMeshProUGUI conferences;
    TextMeshProUGUI books;
    TextMeshProUGUI thesis;
    TextMeshProUGUI patents;
    TextMeshProUGUI research;
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

        journals = dashboardMenu.transform.GetChild(2).GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        conferences = dashboardMenu.transform.GetChild(2).GetChild(2).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        books = dashboardMenu.transform.GetChild(2).GetChild(2).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
        thesis = dashboardMenu.transform.GetChild(2).GetChild(2).GetChild(2).GetChild(3).GetComponent<TextMeshProUGUI>();
        patents = dashboardMenu.transform.GetChild(2).GetChild(2).GetChild(2).GetChild(4).GetComponent<TextMeshProUGUI>();
        research = dashboardMenu.transform.GetChild(2).GetChild(2).GetChild(2).GetChild(5).GetComponent<TextMeshProUGUI>();

    }

    // to connect to VR
    private void SubmitIP(string _ip)
    {
        Client.instance.ip = _ip;
    }

    // to connect to database
    private void SetURL(string _ip){
        URL = "https://"+ _ip + ":5000";
    }

    public void ConnectToServer()
    {
        failText.SetActive(false);
        connectButtonText.text = "Connect";
        Client.instance.ConnectToServer();

        // change text to connecting.., set color to black / blue?; then wait
        WaitForNSeconds(3);
    }

    IEnumerator WaitForNSeconds(float n)
    {
        yield return new WaitForSeconds(n);
        if(!connectSuccess){
            failText.SetActive(true);
            connectButtonText.text = "Retry";
        } 
    }

    public void ClientConnected(string _ip){
        connectSuccess = true;
        
        // send phone size for the first time
        ClientSend.SendPhoneSize();

        // set database url
        SetURL(_ip);

        connectMenu.SetActive(false);
        OnTapDashboardMenu();
    }

    public void OnTapDashboardMenu(){
        dashboardPanel.SetActive(true);
        dashboardErrorPanel.SetActive(false);
        dashboardMenu.SetActive(true);

        // get dashboard data
        requestPeneliti.URL = URL;
        StartCoroutine(requestPeneliti.RequestData((result) =>
        {
            // mengambil jumlah jurnal, conference, books, thesis, paten dan research yang ada
            hasilPublikasiITS(result);
            
        }, (error) => {
            if (error != "")
            {
                dashboardPanel.SetActive(false);
                dashboardErrorPanel.SetActive(true);
            }
        }));

        ClientSend.SendTexture();
    }

    private void hasilPublikasiITS(RawData rawdata)
    {
        journals.text = rawdata.data[0].dashboard_data[0].hasil_publikasi[0].journals.ToString();
        conferences.text = rawdata.data[0].dashboard_data[0].hasil_publikasi[1].conferences.ToString();
        books.text = rawdata.data[0].dashboard_data[0].hasil_publikasi[2].books.ToString();
        thesis.text = rawdata.data[0].dashboard_data[0].hasil_publikasi[3].thesis.ToString();
        patents.text = rawdata.data[0].dashboard_data[0].hasil_publikasi[4].paten.ToString();
        research.text = rawdata.data[0].dashboard_data[0].hasil_publikasi[5].research.ToString();

        ClientSend.SendTexture();
    }

    public void OnTapFilterMenu(){
        filterMenu.SetActive(true);
        ClientSend.SendTexture();
    }

    public void GetResearcherDetailData(string _id){
        requestPeneliti.URL = URL + "/detailpeneliti?id_peneliti=" + _id;
        StartCoroutine(requestPeneliti.RequestData((result) =>
        {
            // mengambil jumlah jurnal, conference, books, thesis, paten dan research yang ada
            detailPenelitiITS(result);
        }, (error) => {
            if (error != "")
            {
                detailPanel.SetActive(false);
                detailErrorPanel.SetActive(true);
            }
        }));
    }

    public void detailPenelitiITS(RawData rawdata)
    {   
        List<string> detail = new List<string>();
        detail.Add(rawdata.data[0].detail_peneliti[0].nama);
        detail.Add(rawdata.data[0].detail_peneliti[0].tanggal_lahir);
        detail.Add(rawdata.data[0].detail_peneliti[0].fakultas);
        detail.Add(rawdata.data[0].detail_peneliti[0].departemen);

        detail.Add(rawdata.data[0].detail_peneliti[0].jurnal.ToString());
        detail.Add(rawdata.data[0].detail_peneliti[0].konferensi.ToString());
        detail.Add(rawdata.data[0].detail_peneliti[0].buku.ToString());
        detail.Add(rawdata.data[0].detail_peneliti[0].tesis.ToString());
        detail.Add(rawdata.data[0].detail_peneliti[0].paten.ToString());
        detail.Add(rawdata.data[0].detail_peneliti[0].penelitian.ToString());

        FilterManager.instance.UpdateResearcherDetail(detail);
    }

    public void OnTapSettingsMenu(){
        settingsMenu.SetActive(true);
        ClientSend.SendTexture();
    }

    private void CopyRenderTextureTo2D(){
        if(textureToSend2D != null){
            DestroyImmediate(textureToSend2D);
        }
        
        RenderTexture oriTexture = renderCamera.targetTexture;
        RenderTexture textureToSend = new RenderTexture(oriTexture.width, oriTexture.height, oriTexture.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        renderCamera.targetTexture = textureToSend;
        renderCamera.Render();
        RenderTexture.active = textureToSend;

        textureToSend2D = new Texture2D(textureToSend.width, textureToSend.height, TextureFormat.ARGB32, false);
        textureToSend2D.ReadPixels(new Rect(0, 0, textureToSend.width, textureToSend.height), 0, 0);
        textureToSend2D.Apply();

        renderCamera.targetTexture = oriTexture;
        renderCamera.Render();
        RenderTexture.active = oriTexture;

        DestroyImmediate(textureToSend);
    }

    public float GetScreenWidth(){
        return Screen.width / Screen.dpi;
    }
    public float GetScreenHeight(){
        return Screen.height / Screen.dpi;
    }
    public byte[] GetTexture(){
        CopyRenderTextureTo2D();
        return textureToSend2D.EncodeToPNG();
    }


    
}
