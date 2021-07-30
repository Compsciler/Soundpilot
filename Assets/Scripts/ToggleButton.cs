using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    protected string prefsKey = null;  // Optional: set this in derived class

    [SerializeField] bool defaultValue;
    protected bool value;

    [SerializeField] Image buttonImage;

    [SerializeField] Sprite toggleOffSprite;
    [SerializeField] Sprite toggleOnSprite;

    void Awake()
    {
        InitializeValue();
    }

    public void Toggle()
    {
        value = !value;
        if (prefsKey != null)
        {
            if (PlayerPrefs.GetInt(prefsKey) == 1)
            {
                PlayerPrefs.SetInt(prefsKey, 0);
            }
            else
            {
                PlayerPrefs.SetInt(prefsKey, 1);
            }
        }
        Display();
    }

    public virtual void Display()
    {
        if (value)
        {
            buttonImage.sprite = toggleOnSprite;
        }
        else
        {
            buttonImage.sprite = toggleOffSprite;
        }
    }

    public void InitializeValue()
    {
        if (prefsKey == null)
        {
            value = defaultValue;
        }
        else
        {
            if (PlayerPrefs.HasKey(prefsKey))
            {
                if (PlayerPrefs.GetInt(prefsKey) == 1)
                {
                    value = true;
                }
                else
                {
                    value = false;
                }
            }
            else
            {
                if (value)
                {
                    PlayerPrefs.SetInt(prefsKey, 1);
                }
                else
                {
                    PlayerPrefs.SetInt(prefsKey, 0);
                }
            }
        }
    }
}