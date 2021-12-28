using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginTab : MonoBehaviour
{
    public InputField id;
    public InputField pwd;
    public void OpenLoginPanel()
    {
        gameObject.SetActive(true);
    }

    public void CloseLoginPanel()
    {
        id.text = "";
        pwd.text = "";
        gameObject.SetActive(false);
    }
}
