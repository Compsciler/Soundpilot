using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class UsernameCreation : MonoBehaviour
{
    //{ Dreamlo username leaderboard private and public code pair
    private string privateCode = "gj0QR61YOUyLiGs_ZVIdFw8m_l4jEEW0SytEJWpGyD0g";
    private string publicCode = "5f2ba017eb371809c4afd909";
    private string webURL = "http://dreamlo.com/lb/";

    [SerializeField] GameObject mainMenu;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] BeforeMainMenuLoaded beforeMainMenuLoadedScript;

    private string inputUsername;
    private bool isDoneWritingUsername = false;
    private bool checkIfUsernameIsUniqueFinished = false;
    private bool isConnectionTimedOut = false;

    private int minLength = 3;
    private int maxLength = 20;
    private bool[] reqMetArr = {true, false, false};

    public IEnumerator CreateUsername()  // Usernames may only contain [A-Z][a-z][0-9]_ and must be unique by lowercase
    {
        mainMenu.SetActive(false);
        gameObject.SetActive(true);

        do
        {
            isDoneWritingUsername = false;
            checkIfUsernameIsUniqueFinished = false;
            isConnectionTimedOut = false;

            yield return new WaitUntil(() => isDoneWritingUsername);
            errorText.text = "";
            inputUsername = inputField.text;
            reqMetArr[1] = AreCharactersValid();
            reqMetArr[2] = IsLengthValid();
            if (reqMetArr[1] && reqMetArr[2])
            {
                StartCoroutine(CheckIfUsernameIsUnique());
                yield return new WaitUntil(() => checkIfUsernameIsUniqueFinished);
                if (isConnectionTimedOut || !string.IsNullOrEmpty(errorText.text))
                {
                    errorText.text = "Check your internet connection and try again.";
                    reqMetArr[0] = false;
                    continue;
                }
            }
            errorText.text = "Requirement(s) ";
            for (int i = 0; i < reqMetArr.Length; i++)
            {
                if (!reqMetArr[i])
                {
                    errorText.text += (i + 1) + ", ";
                }
            }
            errorText.text = errorText.text.Substring(0, errorText.text.Length - 2);  // Removes last ", "
            errorText.text += " failed!";
        }
        while (!reqMetArr.All(b => b));
        errorText.gameObject.SetActive(false);
        StartCoroutine(UploadAndSetNewUsername());
    }

    IEnumerator UploadAndSetNewUsername()
    {
        UnityWebRequest request = UnityWebRequest.Get(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(inputUsername.ToLower()) + "/" + 1);
        yield return request.SendWebRequest();

        if (string.IsNullOrEmpty(request.error))
        {
            PlayerPrefs.SetString(Constants.prefsUsername, inputUsername);
            LeaderboardManager.username = inputUsername;
            beforeMainMenuLoadedScript.isReadyToLoadMainMenu = true;
            gameObject.SetActive(false);
        }
        else
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Error uploading username! Restart app and try again.";
        }
    }

    public void SetIsDoneWritingUsername(bool value)
    {
        isDoneWritingUsername = value;
    }

    IEnumerator CheckIfUsernameIsUnique()
    {
        UnityWebRequest request = UnityWebRequest.Get(webURL + publicCode + "/pipe-get/" + inputUsername.ToLower());  // Gets "score" for inputUsername if inputUsername exists
        request.timeout = Constants.connectionTimeoutTime;
        yield return request.SendWebRequest();

        errorText.text = request.error;
        reqMetArr[0] = (string.IsNullOrEmpty(request.downloadHandler.text));
        checkIfUsernameIsUniqueFinished = true;
    }

    bool AreCharactersValid()
    {
        Regex re = new Regex("^[A-Za-z0-9_]*$");
        return re.IsMatch(inputUsername);
    }

    bool IsLengthValid()
    {
        return (inputUsername.Length >= minLength && inputUsername.Length <= maxLength);
    }

    public void PlayAsGuest()  // Temporarily sets username as guest username
    {
        LeaderboardManager.isPlayingAsGuest = true;
        beforeMainMenuLoadedScript.isReadyToLoadMainMenu = true;
        gameObject.SetActive(false);
    }
}