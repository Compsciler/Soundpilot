using System;
using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    [SerializeField] AudioClip defaultSound;
    [SerializeField] AudioClip backSound;
    [SerializeField] AudioClip mainNavigationSound;
    [SerializeField] AudioClip otherSound1;

    // [SerializeField] AudioClip[] clickSounds;

    public enum ClickSound
    {
        Default,
        Back,
        MainNavigation,
        Other1,
        Custom
    }

    public void PlaySound(AudioClip sound)
    {
        PlaySound(sound, 1f);
    }
    public void PlaySound(AudioClip sound, float volumeScale)
    {
        AudioManager.instance.SFX_Source.PlayOneShot(sound, volumeScale);
    }
    public void PlaySound(ClickSound clickSound)
    {
        PlaySound(clickSound, 1f);
    }
    public void PlaySound(ClickSound clickSound, float volumeScale)
    {
        switch (clickSound)
        {
            case ClickSound.Default:
                PlaySound(defaultSound, volumeScale);
                break;
            case ClickSound.Back:
                PlaySound(backSound, volumeScale);
                break;
            case ClickSound.MainNavigation:
                PlaySound(mainNavigationSound, volumeScale);
                break;
            case ClickSound.Other1:
                PlaySound(otherSound1, volumeScale);
                break;
        }
    }
    /*
    public void PlaySound(int soundNum)
    {
        AudioManager.instance.SFX_Source.PlayOneShot(clickSounds[soundNum]);
    }
    */
}