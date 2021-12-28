using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseTab : MonoBehaviour
{

    public Text whiteMoraleText;
    public Text blackMoraleText;

    public Text whiteNameText;
    public Text blackNameText;

    public Text whiteScoreText;
    public Text blackScoreText;


    public void OpenPausePanel()
    {
        SetUserData((GameManager.Instance.myName, GameManager.Instance.rivalName), (GameManager.Instance.myScore, GameManager.Instance.rivalScore));
        gameObject.SetActive(true);
    }
    public void ClosePausePanel()
    {
        gameObject.SetActive(false);
    }

    public void SetUserData((string, string) names, (string, string) scores)
    {
        whiteNameText.text = names.Item1;
        blackNameText.text = names.Item2;

        whiteScoreText.text = scores.Item1;
        blackScoreText.text = scores.Item2;
    }

    public void SetMorales((string, string) morales)
    {
        whiteMoraleText.text = morales.Item1;
        blackMoraleText.text = morales.Item2;
    }
}
