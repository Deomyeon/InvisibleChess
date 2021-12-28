using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultTab : MonoBehaviour
{

    public Text result;

    public UnityEngine.Color red;
    public UnityEngine.Color green;

    public Text shadow;

    System.Action act;

    public void OpenResultPanel()
    {
        gameObject.SetActive(true);
    }
    public void CloseResultPanel()
    {
        gameObject.SetActive(false);
    }

    public void SetResult(bool victory, System.Action act)
    {
        if (victory)
        {
            result.text = "½Â¸®";
            shadow.text = "½Â¸®";
            result.color = green;
        }
        else
        {
            result.text = "ÆÐ¹è";
            shadow.text = "ÆÐ¹è";
            result.color = red;
        }
        this.act = act;
    }
    
    public void Exit()
    {
        if (act != null) act();
    }
}
