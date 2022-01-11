using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/* SCRIPT UNTUK APLIKASI DI SMARTPHONE */
public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] private RectTransform uiHandleRectTransform;
    [SerializeField] private Color backgroundActiveColor;

    private Image backgroundImage;
    private Color backgroundDefaultColor;
    private Toggle toggle;
    private Vector2 handlePosition;

    void Awake(){
        toggle = GetComponent<Toggle>();
        handlePosition = uiHandleRectTransform.anchoredPosition;
        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        backgroundDefaultColor = backgroundImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);

        if(toggle.isOn){
            OnSwitch(true);
        }
    }

    void OnSwitch(bool on){
        uiHandleRectTransform.anchoredPosition = on? handlePosition * -1 : handlePosition;
        backgroundImage.color = on? backgroundActiveColor : backgroundDefaultColor;

        // toggle dashboard in VR
        ClientSend.SendDashboardToggle(on);

        // send texture
        ClientSend.SendTexture();
    }

    void OnDestroy(){
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
