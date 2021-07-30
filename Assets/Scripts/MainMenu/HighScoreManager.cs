using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoreManager : MonoBehaviour
{
    internal static HighScoreManager instance;
    [SerializeField] internal int gameMode = -1;

    internal string[] highScoreStrings = {"GM0HighScore", "GM1HighScore", "GM2HighScore", "GM3HighScore", "GM4HighScore", "GM5HighScore", "GM6HighScore", "GM7HighScore"};  //{ Optional: rename

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public int[] GetLeaderboardHighScoreSums(int[][] highScoreSums)
    {
        int[] leaderboardHighScores = new int[highScoreSums.Length];
        for (int i = 0; i < highScoreSums.Length; i++)
        {
            leaderboardHighScores[i] = GetHighScoreSum(highScoreSums[i]);
        }
        return leaderboardHighScores;
    }

    public int[] GetHighScores()
    {
        int[] highScores = new int[highScoreStrings.Length];
        for (int i = 0; i < highScoreStrings.Length; i++)
        {
            highScores[i] = GetHighScore(i);
        }
        return highScores;
    }

    public int GetHighScore(int gameMode)
    {
        return PlayerPrefs.GetInt(highScoreStrings[gameMode], 0);
    }
    public void SetHighScore(int gameMode, int newScore)
    {
        PlayerPrefs.SetInt(highScoreStrings[gameMode], newScore);
    }

    public int GetHighScoreSum(int[] gameModes)
    {
        int[] highScores = GetHighScores();
        int highScoreSum = 0;
        for (int i = 0; i < highScores.Length; i++)
        {
            if (gameModes.Contains(i))
            {
                highScoreSum += highScores[i];
            }
        }
        return highScoreSum;
    }

    public void UpdateHighScore(int newScore, bool isUpdatingToNewScore)
    {
        int highScore = GetHighScore(gameMode);

        if ((newScore > highScore) || isUpdatingToNewScore)
        {
            SetHighScore(gameMode, newScore);
            Debug.Log(highScoreStrings[gameMode] + " changed from " + highScore + " to " + newScore);
        }
    }

    public void ResetHighScores()
    {
        for (int i = 0; i < highScoreStrings.Length; i++)
        {
            SetHighScore(i, 0);
        }
        Debug.Log("High scores reset!");
    }

    public void UnlockAllGameModes(int value)
    {
        PlayerPrefs.SetInt(Constants.prefsAreAllGameModesUnlocked, value);  // Restart needed if switching setting from 1 to 0
    }
}