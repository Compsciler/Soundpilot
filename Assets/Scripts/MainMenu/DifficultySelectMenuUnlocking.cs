using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultySelectMenuUnlocking : DifficultySelectMenu
{
    [SerializeField] GameObject difficultyButtonsHolder;
    [SerializeField] GameObject scoreTextsHolder;
    [SerializeField] GameObject lockIconsHolder;

    private GameObject[] difficultyButtons;
    private GameObject[] scoreTexts;
    private GameObject[] lockIcons;

    // https://stackoverflow.com/questions/5849548/is-this-array-initialization-incorrect
    //{ Game mode unlock requirements, in order by the game mode (starting from tutorial here) considered for requirements
    internal static int[][,] gameModeUnlockReqs = new int[][,]{
        new int[,] {{}},
        new int[,] {{}},
        new int[,] {{0, 20}},
        new int[,] {{1, 25}},
        new int[,] {{0, 20}},
        new int[,] {{3, 25}},
        new int[,] {{0, 20}},
        new int[,] {{5, 25}},
        new int[,] {{2, 30}, {4, 20}, {6, 35}}
    };

    void OnEnable()
    {
        SetUpUnlocksAndScores();
    }

    private void SetUpUnlocksAndScores()
    {
        int[] highScores = HighScoreManager.instance.GetHighScores();
        int highScoreIndex = 0;

        for (int i = 0; i < gameModeUnlockReqs.Length; i++)
        {
            int[,] currentUnlockReqs = gameModeUnlockReqs[i];
            bool currentUnlockReqsMet = true;
            for (int j = 0; j < currentUnlockReqs.Length / 2; j++)  // Foreach loop doesn't work somehow, probably because C# Length property returns total number of integers in array, could change to .GetLength(0)
            {
                int highScoreForReq = highScores[currentUnlockReqs[j, 0]];
                int minScoreReq = currentUnlockReqs[j, 1];
                if (highScoreForReq < minScoreReq)
                {
                    currentUnlockReqsMet = false;
                }
            }
            if (currentUnlockReqsMet || PlayerPrefs.GetInt(Constants.prefsAreAllGameModesUnlocked, 0) == 1)
            {
                lockIcons[i].SetActive(false);
                try
                {
                    startButtons[i].GetComponent<Button>().interactable = true;
                }
                catch (NullReferenceException)
                {

                }
                if (difficultyButtons[i].GetComponent<DifficultyButton>().hasHighScore)  // Does not access scores of Tutorial and Custom Mode  //{ Optional: change if needed
                {
                    int highScore = highScores[highScoreIndex];
                    highScoreIndex++;
                    if (highScore > 0)
                    {
                        TMP_Text scoreText = scoreTexts[i].GetComponent<TMP_Text>();
                        scoreText.text = highScore.ToString();
                    }
                }
            }
        }
    }

    protected override void SetDifficultyButtonRelatedUI()
    {
        base.SetDifficultyButtonRelatedUI();
        difficultyButtons = difficultyButtonsHolder.GetChildren();
        scoreTexts = scoreTextsHolder.GetChildren();
        lockIcons = lockIconsHolder.GetChildren();
    }
}