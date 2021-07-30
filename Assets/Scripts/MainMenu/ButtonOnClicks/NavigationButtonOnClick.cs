using UnityEngine;

public class NavigationButtonOnClick : ButtonOnClick
{
    [SerializeField] GameObject currentMenu;
    [SerializeField] GameObject nextMenu;

    public override void OnClickFunction()
    {
        base.OnClickFunction();
        currentMenu.SetActive(false);
        nextMenu.SetActive(true);
    }
}