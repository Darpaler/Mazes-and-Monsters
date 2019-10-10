using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Button startRace;
    public Text coinArea;
    public List<RacerUIElement> racerUIElements; // Parallel to GameManager.instance.AIList
    public bool allowBets = true;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoins();
        UpdateRacers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonPressed()
    {
        startRace.enabled = false;
    }

    public void UpdateCoins()
    {
        coinArea.text = "Creepy Cash: $" + GameManager.instance.player.cashMoney;
    }


    public void DisableBetting() { allowBets = false;  }
    public void EnableBetting() { allowBets = true;  }
    
    public void IncreaseBet(int characterIndex) {
        if (!allowBets) return;

        if (GameManager.instance.player.cashMoney > 1)
        {
            GameManager.instance.player.cashMoney -= 1;
            UpdateCoins();

            GameManager.instance.player.betList[characterIndex] += 1;
            racerUIElements[characterIndex].racer.currentBet = GameManager.instance.player.betList[characterIndex];
            racerUIElements[characterIndex].UpdateUI();
        }
    }
    public void DecreaseBet(int characterIndex) {
        if (!allowBets) return;

        if (GameManager.instance.player.betList[characterIndex] > 1)
        {
            GameManager.instance.player.betList[characterIndex] -= 1;
            racerUIElements[characterIndex].racer.currentBet = GameManager.instance.player.betList[characterIndex];
            racerUIElements[characterIndex].UpdateUI();

            GameManager.instance.player.cashMoney += 1;
            UpdateCoins();
        }
    }

    public void UpdateRacers() 
    {
        for (int i=0; i<racerUIElements.Count; i++) 
        {
            if (i < GameManager.instance.racers.Count)
            {
                racerUIElements[i].gameObject.SetActive(true);
                racerUIElements[i].racer = GameManager.instance.racers[i];
                racerUIElements[i].UpdateUI();
            }
            else
            {
                racerUIElements[i].gameObject.SetActive(false);
            }
        }
    }

}
