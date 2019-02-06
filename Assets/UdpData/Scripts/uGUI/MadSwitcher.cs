using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class MadSwitcher : MonoBehaviour
{
    private MadController madController;
    private ToggleGroup toggleGroup;
    private Toggle[] toggleList;
    public int selectedNumber = 0;
    void Start()
    {
        madController = GameObject.Find("MadController").GetComponent<MadController>();     // コントロール取得
        madController.OnSurfaceNameChanged.AddListener(onSurfaceNameChanged);               // リスナー登録

        toggleGroup = GetComponent<ToggleGroup>();                                                  // グループの取得
        toggleList = toggleGroup.GetComponentsInChildren<Toggle>();                                 // 子のリスト取得
        foreach ( var obj in toggleList.Select((toggle, index) => new {toggle, index})){
            UnityAction<bool> action = (bool value) => onValueChanged(value, obj.index);            // アクション作成
            obj.toggle.onValueChanged.AddListener(action);                                          // 子にリスナー登録
            //Debug.Log("ChildrenToggleName: " + obj.toggle.name);    // 子の名前一覧を出力
        }

        foreach (Toggle t in toggleList) {
            int flag = 0;
            if (t.isOn){
                flag = 1;
            }else{
                flag = 0;
            }
            //mc.send();
        }

        // string selectedLabel = toggleGroup.ActiveToggles().First().name;     // アクティブなものを検索
    }
    void onValueChanged(bool value, int index) {
        selectedNumber = index;
        int flag = 0;
        if (value){
            flag = 1;
        }else{
            flag = 0;
        }
        string name = madController.SurfaceNameList[index];
        madController.send("/surfaces/" + name + "/visible", flag);
        Debug.Log("Selected: " + name);
    }

    void onSurfaceNameChanged(string[] surfaceName) {
        for(int i=0; i<toggleList.Length; i++) {
            if (surfaceName.Length > i) {
                toggleList[i].enabled = true;
                toggleList[i].GetComponentInChildren<Text>().text = surfaceName[i];
            }else{
                toggleList[i].enabled = false;
                toggleList[i].GetComponentInChildren<Text>().text = "";
            }
        }
    }
}
