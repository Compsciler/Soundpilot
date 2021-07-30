using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MEC;

public class DifficultySelectMenu : MonoBehaviour
{
    [SerializeField] GameObject pressedButtonImagesHolder;
    [SerializeField] GameObject startButtonsHolder;
    [SerializeField] GameObject descriptionTextsHolder;

    private GameObject[] pressedButtonImages;
    protected GameObject[] startButtons;
    private GameObject[] descriptionTexts;

    [SerializeField] GameObject fadingMask;
    [SerializeField] float fadeTime;
    [SerializeField] GameObject[] enableAfterFading;

    [SerializeField] GameObject[] enableOnFirstTimePlaying;

    void Awake()
    {
        SetDifficultyButtonRelatedUI();
    }

    void Start()
    {
        if (PlayerPrefs.GetInt(Constants.prefsIsFirstTimePlaying, 1) == 1)
        {
            SetEachActive(enableOnFirstTimePlaying, true);
            PlayerPrefs.SetInt(Constants.prefsIsFirstTimePlaying, 0);
        }
    }

    public void Play(int gameMode)
    {
        Timing.RunCoroutine(PlayStartCoroutine());
        AudioManager.instance.musicSource.Stop();

        HighScoreManager.instance.gameMode = gameMode;
    }

    IEnumerator<float> PlayStartCoroutine()
    {
        fadingMask.SetActive(true);
        CoroutineHandle fadeBackgroundCoroutine = Timing.RunCoroutine(FadeBackground());
        yield return Timing.WaitUntilDone(fadeBackgroundCoroutine);
        SetEachActive(enableAfterFading, true);
        SceneManager.LoadSceneAsync(Constants.gameSceneBuildIndex);
    }

    IEnumerator<float> FadeBackground()
    {
        float timer = 0;
        while (timer < fadeTime)
        {
            Color maskColor = fadingMask.GetComponent<Image>().color;
            fadingMask.GetComponent<Image>().color = new Color(maskColor.r, maskColor.g, maskColor.b, Mathf.Lerp(0, 1, timer / fadeTime));
            timer += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }

    public void ResetMenuPresses()
    {
        SetEachActive(pressedButtonImages, false);
        try
        {
            SetEachActive(startButtons, false);
        }
        catch (NullReferenceException)
        {

        }
        SetEachActive(descriptionTexts, false);
    }

    protected virtual void SetDifficultyButtonRelatedUI()
    {
        pressedButtonImages = pressedButtonImagesHolder.GetChildren();
        startButtons = startButtonsHolder.GetChildren();
        descriptionTexts = descriptionTextsHolder.GetChildren();
    }

    private void SetEachActive(GameObject[] gameObjects, bool value)
    {
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(value);
        }
    }
}