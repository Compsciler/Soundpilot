using UnityEngine;

public class PlayButtonOnClick : ButtonOnClick
{
    [SerializeField] int gameMode;

    [SerializeField] GameObject difficultySelectMenu;

    public override void OnClickFunction()
    {
        base.OnClickFunction();
        difficultySelectMenu.GetComponent<DifficultySelectMenuUnlocking>().Play(gameMode);
    }
}