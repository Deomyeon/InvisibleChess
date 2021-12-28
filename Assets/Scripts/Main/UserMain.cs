using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserMain : MonoBehaviour
{
    public UserTab userTab;
    public StatTab statTab;
    public StateTab stateTab;
    public ErrorTab errorTab;
    public LoadingTab loadingTab;


    public AudioClip matchingSound;

    System.Threading.Thread matching = null;

    private void Start()
    {
        stateTab.match.onClick.AddListener(SearchMatch);
        stateTab.match.GetComponentInChildren<Text>().text = "매치 플레이";

        loadingTab.OpenLoadingPanel();
        Invoke("GetStat", 0.1f);
        
    }

    private void GetStat()
    {
        GameManager.Instance.CallQuery($"Stat~{GameManager.Instance.Token}#", (s) =>
        {
            string[] ss = s.Split('~');
            string[] ts = ss[1].Split('#');

            if (ts[0] == "1")
            {
                statTab.SetStatPanel(ts[1], ts[2], ts[3], ts[4], ts[5]);
                double tempWin = double.Parse(ts[2]);
                double tempLose = double.Parse(ts[3]);
                userTab.SetUserPanel(ts[1], $"{ ((tempWin == 0) ? 0 : (tempWin / double.Parse(ts[4])) * 100).ToString("00.00")}%", ts[5]);
            }
            else
            {
                PrintError(ss[0], ts[1]);
            }
            InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.End();
            loadingTab.CloseLoadingPanel();
        });
    }

    private void PrintError(string protocol, string errorContext)
    {
        string errorMsg = "";

        switch (protocol)
        {
            case "Stat":
                switch (errorContext)
                {
                    case "InValid_Token":
                        errorMsg = "인증되지 않은 사용자입니다.";
                        GameManager.Instance.Token = string.Empty;
                        break;
                    default:
                        errorMsg = "무언가 잘못되었습니다.";
                        break;
                }
                break;
            case "InMatch":
                InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.End();
                switch (errorContext)
                {
                    case "Already_Matching":
                        errorMsg = "이미 매칭중입니다.";
                        errorTab.OnClose = () =>
                        {
                            stateTab.match.onClick.RemoveListener(SearchMatch);
                            stateTab.match.onClick.AddListener(CancelSearch);
                            stateTab.match.GetComponentInChildren<Text>().text = "매치 취소";
                            errorTab.OnClose = null;
                        };
                        break;
                    case "InValid_Token":
                        errorMsg = "인증되지 않은 사용자입니다.";
                        GameManager.Instance.Token = string.Empty;
                        break;
                    default:
                        errorMsg = "무언가 잘못되었습니다.";
                        break;
                }
                break;
            case "OutMatch":
                switch (errorContext)
                {
                    case "Not_While_Matching":
                        errorMsg = "매칭중이 아닙니다.";
                        errorTab.OnClose = () =>
                        {
                            stateTab.match.onClick.RemoveListener(CancelSearch);
                            stateTab.match.onClick.AddListener(SearchMatch);
                            stateTab.match.GetComponentInChildren<Text>().text = "매치 플레이";
                            errorTab.OnClose = null;
                        };
                        break;
                    case "InValid_Token":
                        errorMsg = "인증되지 않은 사용자입니다.";
                        GameManager.Instance.Token = string.Empty;
                        break;
                    default:
                        errorMsg = "무언가 잘못되었습니다.";
                        break;
                }
                break;
            case "TimeOut":
                errorMsg = "연결 시간이 초과되었습니다.";
                errorTab.OnClose = () =>
                {
                    Exit();
                    errorTab.OnClose = null;
                };
                break;
            default:
                errorMsg = "무언가 잘못되었습니다.";
                break;
        }

        errorTab.errorMsg.text = errorMsg;
        errorTab.OpenErrorPanel();
    }

    public void SearchMatch()
    {
        if (matching == null)
        {
            loadingTab.OpenLoadingPanel();
            GameManager.Instance.CallQuery($"InMatch~{GameManager.Instance.Token}#", (s) =>
            {
                string[] ss = s.Split('~');
                string[] ts = ss[1].Split('#');

                if (ts[0] == "1")
                {
                    GameManager.Instance.client = InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.GetClient();
                    System.Threading.Thread th = new System.Threading.Thread(() =>
                    {
                        System.Net.Sockets.NetworkStream stream = GameManager.Instance.client.GetStream();


                        while (!stream.DataAvailable) ;
                        byte[] buffer = new byte[1024];
                        int dataSize = stream.Read(buffer, 0, buffer.Length);
                        GameManager.Instance.resultCommandQueue.Enqueue(((s) =>
                        {
                            string[] ss = s.Split('~');
                            
                            if (ss[0] == "Game")
                            {
                                string[] ts = ss[1].Split('#');

                                if (ts[0] == "White")
                                {
                                    GameManager.Instance.roomRole = RoomRole.White;
                                    GameManager.Instance.myName = ts[1];
                                    GameManager.Instance.rivalName = ts[2];
                                    GameManager.Instance.myScore = ts[3];
                                    GameManager.Instance.rivalScore = ts[4];
                                    SceneManager.LoadScene("Game");
                                    SoundManager.Instance.PlaySound(matchingSound, SoundManager.Instance.baseVolume, false).Play();
                                }
                                else if (ts[0] == "Black")
                                {
                                    GameManager.Instance.roomRole = RoomRole.Black;
                                    GameManager.Instance.myName = ts[2];
                                    GameManager.Instance.rivalName = ts[1];
                                    GameManager.Instance.myScore = ts[4];
                                    GameManager.Instance.rivalScore = ts[3];
                                    SceneManager.LoadScene("Game");
                                    SoundManager.Instance.PlaySound(matchingSound, SoundManager.Instance.baseVolume, false).Play();
                                }
                            }
                        }, System.Text.Encoding.UTF8.GetString(buffer, 0, dataSize)));
                        matching = null;
                    });
                    matching = th;
                    th.Start();

                    stateTab.match.onClick.RemoveListener(SearchMatch);
                    stateTab.match.onClick.AddListener(CancelSearch);
                    stateTab.match.GetComponentInChildren<Text>().text = "매치 취소";
                }
                else
                {
                    PrintError(ss[0], ts[1]);
                    InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.End();
                }
                loadingTab.CloseLoadingPanel();
            });
        }
    }

    public void CancelSearch()
    {
        if (matching != null)
        {
            loadingTab.OpenLoadingPanel();
            GameManager.Instance.CallQuery($"OutMatch~{GameManager.Instance.Token}#", (s) =>
            {
                string[] ss = s.Split('~');
                string[] ts = ss[1].Split('#');

                if (ts[0] == "1")
                {
                    if (matching != null)
                    {
                        matching.Abort();
                        matching = null;
                    }
                    if (GameManager.Instance.client != null)
                    {
                        GameManager.Instance.client.Close();
                        GameManager.Instance.client = null;
                    }

                    stateTab.match.onClick.RemoveListener(CancelSearch);
                    stateTab.match.onClick.AddListener(SearchMatch);
                    stateTab.match.GetComponentInChildren<Text>().text = "매치 플레이";
                }
                else
                {
                    PrintError(ss[0], ts[1]);
                }
                InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.End();
                loadingTab.CloseLoadingPanel();
            });
        }
        else
        {
            stateTab.match.onClick.RemoveListener(CancelSearch);
            stateTab.match.onClick.AddListener(SearchMatch);
            stateTab.match.GetComponentInChildren<Text>().text = "매치 플레이";
        }
    }

    public void Exit()
    {
        if (matching != null)
        {
            loadingTab.OpenLoadingPanel();
            GameManager.Instance.CallQuery($"OutMatch~{GameManager.Instance.Token}#", (s) =>
            {
                string[] ss = s.Split('~');
                string[] ts = ss[1].Split('#');

                if (ts[0] == "1")
                {
                    if (matching != null)
                    {
                        matching.Abort();
                        matching = null;
                    }
                    if (GameManager.Instance.client != null)
                    {
                        GameManager.Instance.client.Close();
                        GameManager.Instance.client = null;
                    }
                }
                InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.End();
                loadingTab.CloseLoadingPanel();
                GameManager.Instance.resultCommandQueue.Enqueue(((s) =>
                {
                    SceneManager.LoadScene("Title");
                }, ""));
            });
        }
        else
        {
            GameManager.Instance.resultCommandQueue.Enqueue(((s) =>
            {
                SceneManager.LoadScene("Title");
            }, ""));
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (matching != null)
            {
                Exit();
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (matching != null)
        {
            GameManager.Instance.CallQuery($"OutMatch~{GameManager.Instance.Token}#", (s) =>
            {
                InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.End();
            });
        }
    }
}
