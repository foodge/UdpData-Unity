using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class DebugField : MonoBehaviour
{
    private MadController madController;
    public Text inputText;
    public Button submitButton;
    public Text textBox;

    // Start is called before the first frame update
    void Start()
    {
        madController = GameObject.Find("MadController").GetComponent<MadController>();     // コントロール取得
        madController.OnStringDataRecieved.AddListener(onStringDataRecieved);
        submitButton.onClick.AddListener(onClick);                                         // 子にリスナー登録
    }
    
    void onClick(){
        string str = inputText.text;
        madController.send(str);
        Debug.Log("Clicked: " + str);
    }
    void onStringDataRecieved(string data) {
        textBox.text += "\r\n" + data;
    }
}
