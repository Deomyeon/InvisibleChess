using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HelpTab : MonoBehaviour
{

    public GameObject selectObj;

    public GameObject sightButton;
    public GameObject moraleButton;
    public GameObject desertionButton;

    public GameObject sightPanel;
    public GameObject moralePanel;
    public GameObject desertionPanel;

    bool helpLock;

    public void OpenHelpPanel()
    {
        gameObject.SetActive(true);

        sightPanel.SetActive(true);
        selectObj.transform.position = sightButton.transform.position;
        moralePanel.SetActive(false);
        desertionPanel.SetActive(false);
    }
    public void CloseHelpPanel()
    {
        gameObject.SetActive(false);
    }

    public void OpenSubPanel(string panel)
    {
        if (!helpLock)
        {
            helpLock = true;
            StartCoroutine(SelectButton(panel));
        }
    }

    
    IEnumerator SelectButton(string panel)
    {
        float speed = 0;
        switch (panel)
        {
            case "sight":
                speed = Mathf.Abs(selectObj.transform.position.y - sightButton.transform.position.y);
                break;
            case "morale":
                speed = Mathf.Abs(selectObj.transform.position.y - moraleButton.transform.position.y);
                break;
            case "desertion":
                speed = Mathf.Abs(selectObj.transform.position.y - desertionButton.transform.position.y);
                break;
        }
        speed *= 5;

        while (helpLock)
        {
            switch (panel)
            {
                case "sight":
                    if (Mathf.Abs(selectObj.transform.position.y - sightButton.transform.position.y) < 0.01f)
                    {
                        helpLock = false;

                        sightPanel.SetActive(true);
                        selectObj.transform.position = sightButton.transform.position;
                        moralePanel.SetActive(false);
                        desertionPanel.SetActive(false);
                    }
                    else
                    {
                        selectObj.transform.position = Vector3.MoveTowards(selectObj.transform.position, sightButton.transform.position, speed * Time.deltaTime);
                    }
                    break;
                case "morale":
                    if (Mathf.Abs(selectObj.transform.position.y - moraleButton.transform.position.y) < 0.01f)
                    {
                        helpLock = false;

                        moralePanel.SetActive(true);
                        selectObj.transform.position = moraleButton.transform.position;
                        sightPanel.SetActive(false);
                        desertionPanel.SetActive(false);
                    }
                    else
                    {
                        selectObj.transform.position = Vector3.MoveTowards(selectObj.transform.position, moraleButton.transform.position, speed * Time.deltaTime);
                    }
                    break;
                case "desertion":
                    if (Mathf.Abs(selectObj.transform.position.y - desertionButton.transform.position.y) < 0.01f)
                    {
                        helpLock = false;

                        desertionPanel.SetActive(true);
                        selectObj.transform.position = desertionButton.transform.position;
                        moralePanel.SetActive(false);
                        sightPanel.SetActive(false);
                    }
                    else
                    {
                        selectObj.transform.position = Vector3.MoveTowards(selectObj.transform.position, desertionButton.transform.position, speed * Time.deltaTime);
                    }
                    break;
            }
            yield return null;
        }
    }
}
