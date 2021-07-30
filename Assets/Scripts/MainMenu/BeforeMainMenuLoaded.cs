using UnityEngine;

public class BeforeMainMenuLoaded : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;

    internal bool isReadyToLoadMainMenu = false;
    [SerializeField] GameObject usernameCreationMenu;

    internal static bool isFirstTimeLoadingSinceAppOpened = true;

    void Update()
    {
        if (isReadyToLoadMainMenu)
        {
            isReadyToLoadMainMenu = false;
            if (PlayerPrefs.GetString(Constants.prefsUsername, "").Equals("") && !LeaderboardManager.isPlayingAsGuest)
            {
                StartCoroutine(usernameCreationMenu.GetComponent<UsernameCreation>().CreateUsername());
            }
            else
            {
                if (LeaderboardManager.isPlayingAsGuest)
                {
                    LeaderboardManager.username = Constants.guestUsername;
                }
                else
                {
                    LeaderboardManager.username = PlayerPrefs.GetString(Constants.prefsUsername);
                }
                mainMenu.SetActive(true);
                AudioManager.instance.musicSource.Play();
            }
        }
    }
}