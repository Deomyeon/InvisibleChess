using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ErrorTab : MonoBehaviour
{
    public Text errorMsg;
    public Action OnClose;

    public void OpenErrorPanel()
    {
        gameObject.SetActive(true);
    }

    public void CloseErrorPanel()
    {
        errorMsg.text = "";
        gameObject.SetActive(false);
        if (OnClose != null) OnClose();
    }
}
