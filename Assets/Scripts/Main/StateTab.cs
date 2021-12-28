using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateTab : MonoBehaviour
{
    public Button match;

    public void OpenStatePanel()
    {
        gameObject.SetActive(true);
    }

    public void CloseStatePanel()
    {
        gameObject.SetActive(false);
    }
}
