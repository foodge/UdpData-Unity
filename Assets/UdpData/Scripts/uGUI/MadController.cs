using Osc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MadController : MonoBehaviour
{
    private OscServer server;
    private UdpSender sender;

    public string remoteIp = "192.168.11.22";
    public int remotePort = 8010;
    public int localPort = 8888;
    public int logCapacity = 30;

    void Start()
    {
        server = gameObject.AddComponent<OscServer>();
        sender = gameObject.AddComponent<UdpSender>();

        sender.remoteIp = remoteIp;
        sender.remotePort = remotePort;
        sender.CreateRemoteEP(remoteIp, remotePort);

        server.localPort = localPort;
        server.logCapacity = logCapacity;
        server.StartServer(localPort);

        //Debug.Log(server.onDataReceived);
        server.onDataReceived.AddListener(onDataReceived);                                          // リスナー登録
        getSurfaceNameRequest();
        
    }

    private void onDataReceived(Message message)
    {
        onStringDataRecieved.Invoke(message.ToString());
        /*
        string path = "/surfaces/";
        if (message.path.StartsWith(path)) {
            string str = message.path.Substring(path.Length);                       // パス部分削除
            if (!str.Equals("selected")) {                                          // "selected"除外
                if (str.IndexOf('/') < 0)                                           // 区切り検出
                {
                    Array.Resize(ref surfaceNameList, surfaceNameList.Length + 1);
                    surfaceNameList[surfaceNameList.Length - 1] = str;              // サーフェス名保存
                    Debug.Log(str);
                    onSurfaceNameChanged.Invoke(surfaceNameList);                   // スイッチャーtext書き換え用イベント
                }
            }
        }
        */
        string[] str = message.path.Split('/');
        foreach (string s in str) {
            if (s.Equals("surface")) {
                if (s.Equals("surface")) {

                }
            }
        }
        // getControlValues?url=/surfaces/.*/visual/name&normalized=0
        /*
        foreach (var log in server.OscLog)
        {
            Debug.Log(log.date + log.source + log.oscMsg);
        }
        */
    }

    public void send(string oscPath)
    {
        var osc = new MessageEncoder(oscPath);
        sender.Send(osc.Encode());
    }

    public void send(string oscPath, int value)
    {
        var osc = new MessageEncoder(oscPath);
        osc.Add(value);
        sender.Send(osc.Encode());
    }

    public void getSurfaceNameRequest()
    {
        surfaceNameList = new string[0];
        var osc = new MessageEncoder("/getControls?root=/surfaces&recursive=0");
        sender.Send(osc.Encode());
    }

    private string[] surfaceNameList = new string[0];
    public string[] SurfaceNameList
    {
        get
        {
            return surfaceNameList;
        }
        set
        {
            surfaceNameList = value;
        }
    }

    public class MyStringListEvent : UnityEvent<String[]> { }
    private MyStringListEvent onSurfaceNameChanged = new MyStringListEvent();
    public MyStringListEvent OnSurfaceNameChanged
    {
        get
        {
            return onSurfaceNameChanged;
        }
        set
        {
            onSurfaceNameChanged = value;
        }
    }

    public class MyStringEvent : UnityEvent<String> { }
    private MyStringEvent onStringDataRecieved = new MyStringEvent();
    public MyStringEvent OnStringDataRecieved
    {
        get
        {
            return onStringDataRecieved;
        }
        set
        {
            onStringDataRecieved = value;
        }
    }
}
