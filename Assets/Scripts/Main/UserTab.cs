using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserTab : MonoBehaviour
{
    public new Text name;
    public Text winRate;
    public Text score;

    public void SetUserPanel(string name, string winRate, string score)
    {
        this.name.text = name;
        this.winRate.text = winRate;
        this.score.text = score;
    }
}
