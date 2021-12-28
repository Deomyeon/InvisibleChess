using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionTab : MonoBehaviour
{
    public Slider slider;

    public AudioClip clip;
    public AudioSource audio;
    
    public void OpenOptionPanel()
    {
        slider.value = SoundManager.Instance.baseVolume;
        gameObject.SetActive(true);
    }
    public void CloseOptionPanel()
    {
        gameObject.SetActive(false);
    }


    public void AccountChange()
    {
        GameManager.Instance.Token = string.Empty;
    }

    public void SetVolume()
    {
        SoundManager.Instance.baseVolume = slider.value;
    }

    public void Reading()
    {
        if (audio == null || !audio.isPlaying)
        {
            audio = SoundManager.Instance.PlaySound(clip, SoundManager.Instance.baseVolume, false);
            audio.Play();
        }
    }

}
