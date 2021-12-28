using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterTab : MonoBehaviour
{
    public InputField id;
    public InputField pwd;
    public InputField name;
    public void OpenRegisterPanel()
    {
        gameObject.SetActive(true);
    }

    public void CloseRegisterPanel()
    {
        id.text = "";
        pwd.text = "";
        name.text = "";
        gameObject.SetActive(false);
    }
}
