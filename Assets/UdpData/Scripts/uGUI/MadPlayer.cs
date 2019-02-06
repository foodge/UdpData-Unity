using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class MadPlayer : MonoBehaviour
{
    private Button[] buttonList;
    private MadController madController;
    public MadSwitcher switcher;
    void Start()
    {
        madController = GameObject.Find("MadController").GetComponent<MadController>();
        buttonList = GetComponentsInChildren<Button>();
        foreach ( var obj in buttonList.Select((button, index) => new {button, index})){
            UnityAction action = () => onClick(obj.index);                                  // アクション作成
            obj.button.onClick.AddListener(action);                                         // 子にリスナー登録
            //Debug.Log("ChildrenToggleName: " + obj.button.name);    // 子の名前一覧を出力
        }
    }

    void onClick(int index){
        string path = "default";
        switch (index){
            case 0: // start
                path = "/begin";
                buttonList[1].transform.gameObject.SetActive(true);
                buttonList[0].transform.gameObject.SetActive(false);
                break;
            case 1: // pause
                path = "/pause";
                buttonList[0].transform.gameObject.SetActive(true);
                buttonList[1].transform.gameObject.SetActive(false);
                break;
            case 2: // stop
                path = "/play_forward";
                buttonList[1].transform.gameObject.SetActive(true);
                buttonList[0].transform.gameObject.SetActive(false);
                break;
            default:
                path = "Error";
                break;
        }
        string name = madController.SurfaceNameList[switcher.selectedNumber];
        madController.send("/medias/" + name + path, 1);
        Debug.Log("Clicked: " + name);
    }
}
