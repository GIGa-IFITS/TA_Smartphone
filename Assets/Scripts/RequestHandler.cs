using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class RequestHandler
{
    // Start is called before the first frame update
    public string URL;
    public IEnumerator RequestData(Action<RawData> result, Action<string> message)
    {
        Debug.Log("RequestHandler -> " +URL);
        ForceAcceptAll forceAcceptAll = new ForceAcceptAll();
        UnityWebRequest requestData = UnityWebRequest.Get(URL);
        requestData.certificateHandler = forceAcceptAll;
        yield return requestData.SendWebRequest();
        string jsonResponse = requestData.downloadHandler.text;
        //Debug.Log(jsonResponse);
        RawData rawData = JsonUtility.FromJson<RawData>(jsonResponse);
        if (requestData.isNetworkError)
        {
            Debug.Log("Error: " + requestData.error);
            string errorMsg = "Error: " + requestData.error;
            message(errorMsg);
        }
        else
        {
            if(rawData.code == "500")
            {
                message(rawData.message);
            }else if (rawData.code == "200")
            {
                result(rawData);
            }
        }
    }
}
