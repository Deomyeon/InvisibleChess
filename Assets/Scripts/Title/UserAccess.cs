using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserAccess : MonoBehaviour
{
    public LoginTab loginTab;
    public RegisterTab registerTab;
    public ErrorTab errorTab;
    public LoadingTab loadingTab;
    public new Text name;

    private bool tokenLogin = false;

    public Button accountChangeButton;

    private void Awake()
    {
#if UNITY_STANDALONE
        Screen.SetResolution(360, 640, false);
#endif
    }

    private void Start()
    {
        if (GameManager.Instance.Token != string.Empty)
        {
            loadingTab.OpenLoadingPanel();
            GameManager.Instance.CallQuery($"Stat~{GameManager.Instance.Token}#", (s) =>
            {
                string[] ss = s.Split('~');
                string[] ts = ss[1].Split('#');

                if (ts[0] == "1")
                {
                    name.text = ts[1];
                }
                else
                {
                    PrintError("SomethingWrong", "Something_Wrong");
                }
                tokenLogin = true;
                InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.End();
                loadingTab.CloseLoadingPanel();
            });
        }
        else
        {
            tokenLogin = true;
            accountChangeButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.versionValid)
        {
            errorTab.errorMsg.text = "�ֽ� ������ �ƴմϴ�. \n������Ʈ �ϼ���.";
            errorTab.OnClose = () => { Application.Quit(); };
            errorTab.OpenErrorPanel();
            return;
        }
    }

    public void LoginProccess()
    {
        if (tokenLogin)
        {
            if (GameManager.Instance.Token != string.Empty)
            {
                TokenLogin();
            }
            else
            {
                loginTab.OpenLoginPanel();
            }
        }
    }

    private void PrintError(string protocol, string errorContext)
    {
        string errorMsg = "";

        switch (protocol)
        {
            case "Register":
                switch (errorContext)
                {
                    case "InValid_Id":
                        errorMsg = "��ȿ���� ���� ���̵��Դϴ�.";
                        break;
                    case "InValid_Password":
                        errorMsg = "��ȿ���� ���� ��й�ȣ�Դϴ�.";
                        break;
                    case "InValid_Name":
                        errorMsg = "��ȿ���� ���� �̸��Դϴ�.";
                        break;
                    case "Already_Exists_Id":
                        errorMsg = "�̹� �����ϴ� ���̵��Դϴ�.";
                        break;
                    default:
                        errorMsg = "���� �߸��Ǿ����ϴ�.";
                        break;
                }
                break;
            case "Login":
                switch (errorContext)
                {
                    case "Not_Exists_Account":
                        errorMsg = "���̵� Ȥ�� ��й�ȣ�� Ʋ�Ƚ��ϴ�.";
                        break;
                    default:
                        errorMsg = "���� �߸��Ǿ����ϴ�.";
                        break;
                }
                break;
            case "TokenLogin":
                switch (errorContext)
                {
                    case "InValid_Token":
                        errorMsg = "�������� ���� ������Դϴ�.";
                        GameManager.Instance.Token = string.Empty;
                        break;
                    default:
                        errorMsg = "���� �߸��Ǿ����ϴ�.";
                        break;
                }
                break;
            case "TimeOut":
                errorMsg = "���� �ð��� �ʰ��Ǿ����ϴ�.";
                break;
            default:
                errorMsg = "���� �߸��Ǿ����ϴ�.";
                break;
        }

        errorTab.errorMsg.text = errorMsg;
        errorTab.OpenErrorPanel();
    }

    public void Register()
    {
        loadingTab.OpenLoadingPanel();
        GameManager.Instance.CallQuery($"Register~{registerTab.id.text}#{registerTab.pwd.text}#{registerTab.name.text}#", (s) =>
        {
            string[] ss = s.Split('~');
            string[] ts = ss[1].Split('#');

            if (ts[0] == "1")
            {
                loginTab.OpenLoginPanel();
            }
            else
            {
                PrintError(ss[0], ts[1]);
            }
            InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.End();
            loadingTab.CloseLoadingPanel();
        });
    }

    public void Login()
    {
        loadingTab.OpenLoadingPanel();
        GameManager.Instance.CallQuery($"Login~{loginTab.id.text}#{loginTab.pwd.text}#", (s) =>
        {
            string[] ss = s.Split('~');
            string[] ts = ss[1].Split('#');

            if (ts[0] == "1")
            {
                GameManager.Instance.Token = ts[1];
                GameManager.Instance.resultCommandQueue.Enqueue(((s) =>
                {
                    SceneManager.LoadScene("Main");
                }, ""));
            }
            else
            {
                PrintError(ss[0], ts[1]);
            }
            InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.End();
            loadingTab.CloseLoadingPanel();
        });
    }

    private void TokenLogin()
    {
        loadingTab.OpenLoadingPanel();
        GameManager.Instance.CallQuery($"TokenLogin~{GameManager.Instance.Token}#", (s) =>
        {
            string[] ss = s.Split('~');
            string[] ts = ss[1].Split('#');

            if (ts[0] == "1")
            {
                GameManager.Instance.resultCommandQueue.Enqueue(((s) =>
                {
                    SceneManager.LoadScene("Main");
                }, ""));
            }
            else
            {
                PrintError(ss[0], ts[1]);
            }
            InvisibleChess.SocketManager<InvisibleChess.SocketClient>.Instance.End();
            loadingTab.CloseLoadingPanel();
        });
    }

    public void ChangeAccount()
    {
        GameManager.Instance.Token = string.Empty;
        name.text = "";
        accountChangeButton.gameObject.SetActive(false);
        loginTab.OpenLoginPanel();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
