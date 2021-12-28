using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using InvisibleChess;
using System.Threading;

public enum RoomRole
{
    None,
    White,
    Black
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                obj.AddComponent<GameManager>();
                instance = obj.GetComponent<GameManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    const string serverAddressURL = "https://docs.google.com/spreadsheets/d/1GOUDNEqwUjmIuVUjUAOf-B5vnjyQXjGxrP3o9pSobp4/edit?usp=sharing";

    string datafilePath;

    private string token = string.Empty;
    public string Token
    { 
        get { return token; } 
        set 
        {
            token = value;
            File.WriteAllText(datafilePath, token);
        } 
    }
    string address;
    int port;

    const string version = "0.025";

    public bool versionValid = true;

    public Queue<(Action<string>, string)> resultCommandQueue = new Queue<(Action<string>, string)>();

    bool querying = false;

    public RoomRole roomRole = RoomRole.None;
    public string myName;
    public string rivalName;
    public string myScore;
    public string rivalScore;

    public System.Net.Sockets.TcpClient client;

    private void Update()
    {
        if (resultCommandQueue.Count > 0)
        {
            (Action<string>, string) command = resultCommandQueue.Peek();

            command.Item1(command.Item2);
            resultCommandQueue.Dequeue();
        }
    }

    private void Awake()
    {
        datafilePath = string.Concat(Application.persistentDataPath, "/data_file.txt");

        if (File.Exists(datafilePath))
        {
            token = File.ReadAllText(datafilePath);
        }
        StartConnect();
    }

    public void StartConnect()
    {
        StartCoroutine(RequestServerAddress(
            () =>
            {
                SocketManager<SocketClient>.Instance.SetAll(1024, port, address);
            }
            ));
    }

    public void CallQuery(string text, Action<string> result)
    {
        Thread th = new Thread(() =>
        {
            string getData = string.Empty;
            SocketManager<SocketClient>.Instance.Start(text, (s) => { getData = s; });
            if (getData != string.Empty) resultCommandQueue.Enqueue((result, getData));
        });
        th.Start();
        StartCoroutine(CheckQueryTimeOut(th, (successed) =>
        {
            if (!successed)
            {
                resultCommandQueue.Enqueue((result, "TimeOut~0#TimeOut"));
            }
        }));
    }

    public IEnumerator CheckQueryTimeOut(Thread th, Action<bool> act)
    {
        yield return new WaitForSeconds(5);
        bool threadRunning = th.ThreadState == ThreadState.Running;
        if (threadRunning)
        {
            th.Abort();
        }    
        act(!threadRunning);
    }

    public IEnumerator RequestServerAddress(Action connect)
    {
        UnityWebRequest rq = UnityWebRequest.Get(serverAddressURL);
        yield return rq.SendWebRequest();

        Func<string, string, string> GetUseableData = (data, key) =>
        {
            int begin = data.IndexOf(key) + key.Length;

            StringBuilder sb = new StringBuilder();

            for (int i = begin; data[i] != 10; i++)
            {
                sb.Append(data[i]);
            }

            return sb.ToString();
        };

        string data = rq.downloadHandler.text;
        address = GetUseableData(data, "address,");
        port = int.Parse(GetUseableData(data, "port,"));
        
        if (GetUseableData(data, "version,") == version)
        {
            versionValid = true;
            connect();
        }
        else
        {
            versionValid = false;
        }
    }
}
