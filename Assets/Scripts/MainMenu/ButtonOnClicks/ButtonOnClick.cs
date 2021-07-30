using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnClick : MonoBehaviour
{
    [SerializeField] ButtonClickSound.ClickSound clickSound;
    [SerializeField] AudioClip customClickSound;
    [SerializeField] float clickSoundVolume = 1f;

    [SerializeField] GameObject mainCamera;

    public virtual void OnClickFunction()
    {
        if (clickSound == ButtonClickSound.ClickSound.Custom)
        {
            mainCamera.GetComponent<ButtonClickSound>().PlaySound(customClickSound, clickSoundVolume);
        }
        else
        {
            mainCamera.GetComponent<ButtonClickSound>().PlaySound(clickSound, clickSoundVolume);
        }
    }
}