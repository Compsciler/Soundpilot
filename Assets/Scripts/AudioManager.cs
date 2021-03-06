using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    internal static AudioManager instance;
    internal AudioSource SFX_Source;
    internal AudioSource musicSource;

    [SerializeField] GameObject SFX_MuteButton;
    [SerializeField] GameObject musicMuteButton;
    [SerializeField] Sprite SFX_UnmutedSprite;
    [SerializeField] Sprite SFX_MutedSprite;
    [SerializeField] Sprite musicUnmutedSprite;
    [SerializeField] Sprite musicMutedSprite;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        SFX_Source = GetComponents<AudioSource>()[0];
        musicSource = GetComponents<AudioSource>()[1];
    }

    void Start()
    {
        DisplayCorrectSFX();
        DisplayCorrectMusic();
    }

    public void ToggleSFX()
    {
        if (PlayerPrefs.GetInt(Constants.prefsIsSFX_Muted) == 0)
        {
            PlayerPrefs.SetInt(Constants.prefsIsSFX_Muted, 1);
        }
        else
        {
            PlayerPrefs.SetInt(Constants.prefsIsSFX_Muted, 0);
        }
        DisplayCorrectSFX();
    }

    public void ToggleMusic(bool unpauseOnUnmute)
    {
        if (PlayerPrefs.GetInt(Constants.prefsIsMusicMuted) == 0)
        {
            PlayerPrefs.SetInt(Constants.prefsIsMusicMuted, 1);
        }
        else
        {
            PlayerPrefs.SetInt(Constants.prefsIsMusicMuted, 0);
        }
        DisplayCorrectMusic();
        if (PlayerPrefs.GetInt(Constants.prefsIsMusicMuted) == 0 && unpauseOnUnmute)
        {
            musicSource.UnPause();
        }
    }

    public void DisplayCorrectSFX()
    {
        if (PlayerPrefs.GetInt(Constants.prefsIsSFX_Muted, 0) == 1)
        {
            SFX_Source.mute = true;
            if (SFX_MuteButton != null)
            {
                SFX_MuteButton.GetComponent<Image>().sprite = SFX_MutedSprite;
            }
        }
        else
        {
            SFX_Source.mute = false;
            if (SFX_MuteButton != null)
            {
                SFX_MuteButton.GetComponent<Image>().sprite = SFX_UnmutedSprite;
            }
        }
    }

    public void DisplayCorrectMusic()
    {
        if (PlayerPrefs.GetInt(Constants.prefsIsMusicMuted, 0) == 1)
        {
            musicSource.mute = true;  // Muting to not need checking for all pausing and unpausing situations in the game scene
            musicSource.Pause();
            if (musicMuteButton != null)
            {
                musicMuteButton.GetComponent<Image>().sprite = musicMutedSprite;
            }
        }
        else
        {
            musicSource.mute = false;
            if (musicMuteButton != null)
            {
                musicMuteButton.GetComponent<Image>().sprite = musicUnmutedSprite;
            }
        }
    }
}