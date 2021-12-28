using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingTab : MonoBehaviour
{
    public Animator animator;

    public void OpenLoadingPanel()
    {
        gameObject.SetActive(true);
        animator.Play("Loading");
    }
    public void CloseLoadingPanel()
    {
        gameObject.SetActive(false);
    }    
}
