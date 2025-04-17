using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    public Image iconImage;
    public Sprite emptyIcon;
    public PowerUpIconLibrary iconLibrary;

    public void SetPowerUp(string powerUpName)
    {
        if (string.IsNullOrEmpty(powerUpName))
        {
            iconImage.sprite = emptyIcon;
            iconImage.enabled = false;
        }
        else
        {
            iconImage.sprite = iconLibrary.GetIcon(powerUpName);
            iconImage.enabled = true;
        }
    }
}
