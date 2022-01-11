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
    [SerializeField] private GameObject summaryMenu;
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
    private string currId;
    private string currFilter;
    private string currTag;
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private GameObject detailErrorPanel;

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

        // nodeTag = new Stack<string>();
        // nodeId = new Stack<string>();
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
        detNameText = detailMenu.transform.GetChild(3).GetChild(1).GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        //detBirthText = detailMenu.transform.GetChild(3).GetChild(1).GetChild(0).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        detFacultyText = detailMenu.transform.GetChild(3).GetChild(1).GetChild(0).GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        detDeptText = detailMenu.transform.GetChild(3).GetChild(1).GetChild(0).GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();

        detJournalText = detailMenu.transform.GetChild(3).GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        detConferenceText = detailMenu.transform.GetChild(3).GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        detBookText = detailMenu.transform.GetChild(3).GetChild(1).GetChild(1).GetChild(1).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        detThesisText = detailMenu.transform.GetChild(3).GetChild(1).GetChild(1).GetChild(1).GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        detPatentText = detailMenu.transform.GetChild(3).GetChild(1).GetChild(1).GetChild(1).GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();
        detResearchText = detailMenu.transform.GetChild(3).GetChild(1).GetChild(1).GetChild(1).GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void OnTapFilter(string _filter){
        filterMenu.SetActive(false);
        summaryMenu.SetActive(false);
        illustMenu.SetActive(true);

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
        // nodeTag.Push(_filter);
        // nodeId.Push("-1");

        // foreach( string tag in nodeTag )
        // {
        //     debuggingText.text += tag;
        // }
        // foreach( string id in nodeId )
        // {
        //     debuggingText.text += id;
        // }

        ClientSend.SendCommand(_filter);
        ClientSend.SendTexture();
    }

    public void ShowFilterSummary(string _name, int _total, string _tag, string _nodeId, string _filterName){
        illustMenu.SetActive(false);
        summaryMenu.SetActive(true);

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
        }
        else if(_tag == "ListPublikasiFakultas"){
            summaryInstructionText.text = "Try knocking your phone to nearby node to view researchers with research keyword.";
        }
        else{
            summaryInstructionText.text = "Try knocking your phone to nearby node to view detail of the researcher.";
        }

        currTag = _tag;

        // nodeTag.Push(_tag);
        // nodeId.Push(_nodeId);

        // foreach( string tag in nodeTag )
        // {
        //     debuggingText.text += tag;
        // }
        // foreach( string id in nodeId )
        // {
        //     debuggingText.text += id;
        // }

        Handheld.Vibrate();
        ClientSend.SendTexture();
    }

    public void ShowResearcherDetail(string _id, string _filterName){
        summaryMenu.SetActive(false);
        detailMenu.SetActive(true);

        detFilterText.text = "Filtering By:" + "\n" + _filterName;
        currId = _id;

        NetworkUIManager.instance.GetResearcherDetailData(_id);
    }

    public void RefreshResearcherDetail(){
        detailPanel.SetActive(true);
        detailErrorPanel.SetActive(false);

        NetworkUIManager.instance.GetResearcherDetailData(currId);
    }

    public void UpdateResearcherDetail(List<string> detailData){
        detNameText.text = detailData[0];
        //detBirthText.text = detailData[1];
        detFacultyText.text = detailData[2];
        detDeptText.text = detailData[3];
        detJournalText.text = detailData[4];
        detConferenceText.text = detailData[5];
        detBookText.text = detailData[6];
        detThesisText.text = detailData[7];
        detPatentText.text = detailData[8];
        detResearchText.text = detailData[9];

        Handheld.Vibrate();
        ClientSend.SendTexture();
    }

    public void BackToSummaryMenu(){
        detailMenu.SetActive(false);
        summaryMenu.SetActive(true);

        ClientSend.SendTexture();
    }

    public void BackToFilterMenu(){
        ClientSend.SendCommand("destroy");

        // nodeTag.Clear();
        // nodeId.Clear();

        illustMenu.SetActive(false);
        summaryMenu.SetActive(false);
        filterMenu.SetActive(true);

        ClientSend.SendTexture();
    }

    public void BackToPreviousNode(){
        if(currTag == "ListPenelitiFakultas" || currTag == "ListPublikasiFakultas"){
            // still in summary menu, send current tag and node id
            ClientSend.SendNodeRequest(currId, currTag);
        }
        else{
            // back to illust menu
            OnTapFilter(currFilter);
        }
    }

    // public void BackToPreviousNode(){
    //     nodeTag.Pop();
    //     nodeId.Pop();

    //     foreach( string tag in nodeTag )
    //     {
    //         debuggingText.text += tag;
    //     }
    //     foreach( string id in nodeId )
    //     {
    //         debuggingText.text += id;
    //     }

    //     // back to illust menu
    //     if (nodeId.Peek() == "-1"){
    //         string filter = nodeTag.Peek();
    //         nodeTag.Pop();
    //         nodeId.Pop();
    //         OnTapFilter(filter);
    //     }else{
    //         string _nodeId = nodeId.Peek();
    //         string _nodeTag = nodeTag.Peek();
    //         ClientSend.SendNodeRequest(_nodeId, _nodeTag);
    //         nodeId.Pop();
    //         nodeTag.Pop();
    //     }

    //     ClientSend.SendTexture();
    // }
}
