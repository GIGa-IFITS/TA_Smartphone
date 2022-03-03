using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/* SCRIPT UNTUK APLIKASI DI SMARTPHONE */
public class FilterManager : MonoBehaviour
{
    public static FilterManager instance;
    [SerializeField] private GameObject filterMenu;
    [SerializeField] private GameObject illustMenu;
    [SerializeField] private GameObject illustPanel;
    [SerializeField] private GameObject illustErrorPanel;
    [SerializeField] private GameObject summaryMenu;
    [SerializeField] private GameObject summaryPanel;
    [SerializeField] private GameObject summaryErrorPanel;
    [SerializeField] private GameObject detailMenu;
    private TextMeshProUGUI illustFilterText;
    private TextMeshProUGUI illustInstructionText;
    private TextMeshProUGUI summaryFilterText;
    private TextMeshProUGUI summaryNameText;
    private TextMeshProUGUI summaryTotalText;
    private TextMeshProUGUI summaryVariableText;
    private TextMeshProUGUI summaryInstructionText;
    private TextMeshProUGUI detFilterText;
    private TextMeshProUGUI detNameText;
    //private TextMeshProUGUI detBirthText;
    private TextMeshProUGUI detFacultyText;
    private TextMeshProUGUI detDeptText;
    private TextMeshProUGUI detJournalText;
    private TextMeshProUGUI detConferenceText;
    private TextMeshProUGUI detBookText;
    private TextMeshProUGUI detThesisText;
    private TextMeshProUGUI detPatentText;
    private TextMeshProUGUI detResearchText;
    // private Stack<string> nodeTag;
    // private Stack<string> nodeId;
    public TextMeshProUGUI debuggingText;
    private string prevNodeName;
    private string prevNodeTotal;
    private string prevNodeId;
    private string currFilter;
    private string currTag;
    private string currId;
    private string currId2;
    private string currResearcherId;
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private GameObject detailErrorPanel;
    [SerializeField] private GameObject detailLoading;

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

    private void Start(){
        // filter illust page
        illustFilterText = illustMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        illustInstructionText = illustMenu.transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>();

        // summary page
        summaryFilterText = summaryMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        summaryNameText = summaryMenu.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        summaryTotalText = summaryMenu.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        summaryVariableText = summaryMenu.transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>();
        summaryInstructionText = summaryMenu.transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>();

        // detail page
        detFilterText = detailMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        detNameText = detailPanel.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        //detBirthText = detailPanel.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        detFacultyText = detailPanel.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        detDeptText = detailPanel.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();

        detJournalText = detailPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        detConferenceText = detailPanel.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        detBookText = detailPanel.transform.GetChild(1).GetChild(1).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        detThesisText = detailPanel.transform.GetChild(1).GetChild(1).GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        detPatentText = detailPanel.transform.GetChild(1).GetChild(1).GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();
        detResearchText = detailPanel.transform.GetChild(1).GetChild(1).GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void OnTapFilter(string _filter){
        filterMenu.SetActive(false);
        summaryMenu.SetActive(false);
        illustMenu.SetActive(true);
        illustPanel.SetActive(true);
        illustErrorPanel.SetActive(false);

        currFilter = _filter;
        currTag = "none";

        if(_filter == "name"){
            illustFilterText.text = "Filtering By:" + "\n" + "Researcher Name";
            illustInstructionText.text = "Try knocking your phone to nearby node to view researchers of the same initial.";
        }
        else if(_filter == "unit"){
            illustFilterText.text = "Filtering By:" + "\n" + "Institution Unit";
            illustInstructionText.text = "Try knocking your phone to nearby node to view departements of the faculty.";
        }
        else if(_filter == "degree"){
            illustFilterText.text = "Filtering By:" + "\n" + "Academic Degree";
            illustInstructionText.text = "Try knocking your phone to nearby node to view researcher of the selected academic degree.";
        }
        else if(_filter == "keyword"){
            illustFilterText.text = "Filtering By:" + "\n" + "Research Keyword";
            illustInstructionText.text = "Try knocking your phone to nearby node to view research keywords of the faculty.";
        }

        ClientSend.SendCommand(_filter);
        //ClientSend.SendTexture();
    }

    public void ShowFilterSummary(string _name, int _total, string _tag, string _nodeId, string _filterName){
        illustMenu.SetActive(false);
        summaryMenu.SetActive(true);
        summaryPanel.SetActive(true);
        summaryErrorPanel.SetActive(false);

        summaryFilterText.text = "Filtering By:" + "\n" + _filterName;
        summaryNameText.text = _name;
        summaryTotalText.text = _total.ToString();

        if(_filterName == "Research Keyword"){
            summaryVariableText.text = "Publications";
        }else{
            summaryVariableText.text = "Researchers";
        }

        if(_tag == "ListPenelitiFakultas"){
            summaryInstructionText.text = "Try knocking your phone to nearby node to view researchers of the departement.";
            prevNodeName = _name;
            prevNodeId = _nodeId;
            prevNodeTotal = _total.ToString();
        }
        else if(_tag == "ListPublikasiFakultas"){
            summaryInstructionText.text = "Try knocking your phone to nearby node to view researchers with research keyword.";
            prevNodeName = _name;
            prevNodeId = _nodeId;
            prevNodeTotal = _total.ToString();
        }
        else{
            summaryInstructionText.text = "Try knocking your phone to nearby node to view detail of the researcher.";
        }

        currId = _nodeId;
        currTag = _tag;

        if(_tag == "ListPublikasiKataKunci"){
            currId2 = _name;
        }else{
            currId2 = "0";
        }

        Handheld.Vibrate();
        //ClientSend.SendTexture();
    }

    public void ShowResearcherDetail(string _id, string _filterName){
        summaryMenu.SetActive(false);
        detailMenu.SetActive(true);
        detailLoading.SetActive(true);
        detailPanel.SetActive(false);
        detailErrorPanel.SetActive(false);

        detFilterText.text = "Filtering By:" + "\n" + _filterName;
        currResearcherId = _id;

        NetworkUIManager.instance.GetResearcherDetailData(_id);
        Handheld.Vibrate();
        //ClientSend.SendTexture();

    }

    public void RefreshResearcherDetail(){
        detailLoading.SetActive(true);
        detailPanel.SetActive(false);
        detailErrorPanel.SetActive(false);
        //ClientSend.SendTexture();
        NetworkUIManager.instance.GetResearcherDetailData(currResearcherId);
        ClientSend.SendPageType("retryDetail");
    }

    public void UpdateResearcherDetail(RawData rawdata){
        detailLoading.SetActive(false);
        detailPanel.SetActive(true);
        detailErrorPanel.SetActive(false);

        detNameText.text = rawdata.data[0].detail_peneliti[0].nama;
        detFacultyText.text = rawdata.data[0].detail_peneliti[0].fakultas;
        detDeptText.text = rawdata.data[0].detail_peneliti[0].departemen;
        detJournalText.text = rawdata.data[0].detail_peneliti[0].jurnal.ToString();
        detConferenceText.text = rawdata.data[0].detail_peneliti[0].konferensi.ToString();
        detBookText.text = rawdata.data[0].detail_peneliti[0].buku.ToString();
        detThesisText.text = rawdata.data[0].detail_peneliti[0].tesis.ToString();
        detPatentText.text = rawdata.data[0].detail_peneliti[0].paten.ToString();
        detResearchText.text = rawdata.data[0].detail_peneliti[0].penelitian.ToString();

        //ClientSend.SendTexture();
        ClientSend.SendPageType("researcherDetailData");
    }

    public void ErrorResearcherDetail(){
        detailLoading.SetActive(false);
        detailPanel.SetActive(false);
        detailErrorPanel.SetActive(true);

        //ClientSend.SendTexture();
        ClientSend.SendPageType("errorDetail");
    }

    public void BackToSummaryMenu(){
        detailMenu.SetActive(false);
        summaryMenu.SetActive(true);

        //ClientSend.SendTexture();
        ClientSend.SendPageType("backToSummaryMenu");
    }

    public void BackToFilterMenu(){
        ClientSend.SendCommand("destroy");

        illustMenu.SetActive(false);
        summaryMenu.SetActive(false);
        filterMenu.SetActive(true);

        //ClientSend.SendTexture();
        ClientSend.SendPageType("backToFilterMenu");
    }

    public void BackToPreviousNode(){
        if(currTag == "ListPenelitiDepartemen" || currTag == "ListPublikasiKataKunci"){
            // still in summary menu, send prev id, current tag
            if(currTag == "ListPenelitiDepartemen"){
                currTag = "ListPenelitiFakultas";
            }
            else{
                currTag = "ListPublikasiFakultas";
            }
            currId = prevNodeId;

            ClientSend.SendNodeRequest(currId, "0", currTag);

            summaryNameText.text = prevNodeName;
            summaryTotalText.text = prevNodeTotal;

            Handheld.Vibrate();
            //ClientSend.SendTexture();
            ClientSend.SendPageType("backToPrevNode");
        }
        else{
            // back to illust menu
            OnTapFilter(currFilter);
        }
    }

    public void ShowErrorScreen(string _errorMsg){
        Debug.Log(_errorMsg);

        if(illustMenu.activeSelf){
            illustErrorPanel.SetActive(true);
            illustPanel.SetActive(false);
        }else{
            summaryErrorPanel.SetActive(true);
            summaryPanel.SetActive(false);
        }
        //ClientSend.SendTexture();
    }

    public void RetryFilter(){
        if(currTag == "none"){
            // illust menu
            OnTapFilter(currFilter);
        }else{
            // summary menu
            ClientSend.SendNodeRequest(currId, currId2, currTag);
            summaryPanel.SetActive(true);
            summaryErrorPanel.SetActive(false);
            //ClientSend.SendTexture();
            ClientSend.SendPageType("retryFilter");
        }
    }
}
