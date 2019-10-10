using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RacerUIElement : MonoBehaviour
{
    [Header("Data")]
    public Racer racer;

    [Header("UI Elements")]
    public Text displayNameTextbox;
    public Text currentBetTextbox;
    public Image racerImagebox;

    public void UpdateUI()
    {
        racerImagebox.sprite = racer.racerSprite;
        displayNameTextbox.text = racer.displayName;
        currentBetTextbox.text = ""+racer.currentBet;
    }
}
