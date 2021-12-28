using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatTab : MonoBehaviour
{
    public new Text name;
    public Text winCount;
    public Text loseCount;
    public Text battleCount;
    public Text score;


    public void SetStatPanel(string name, string winCount, string loseCount, string battleCount, string score)
    {
        this.name.text = name;
        this.winCount.text = winCount;
        this.loseCount.text = loseCount;
        this.battleCount.text = battleCount;
        this.score.text = score;
    }

    public void OpenStatPanel()
    {
        gameObject.SetActive(true);
    }

    public void CloseStatPanel()
    {
        gameObject.SetActive(false);
    }
}
