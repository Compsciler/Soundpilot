using UnityEngine;

public class DifficultyButtonOnClick : ButtonOnClick
{
    [SerializeField] GameObject pressedButtonImage;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject descriptionText;

    [SerializeField] GameObject difficultySelectMenu;

    public override void OnClickFunction()
    {
        base.OnClickFunction();
        difficultySelectMenu.GetComponent<DifficultySelectMenuUnlocking>().ResetMenuPresses();
        pressedButtonImage.SetActive(true);
        descriptionText.SetActive(true);

        try
        {
            startButton.SetActive(true);
        }
        catch (UnassignedReferenceException)
        {

        }
    }
}