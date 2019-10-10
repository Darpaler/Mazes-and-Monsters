using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<int> betList; // List of bets, parallel to GameManager.instance.AIList
    public int cashMoney; // Amount of money available to bet
    
    // Start is called before the first frame update
    void Start()
    {
        // Make sure betlist has spot for each racer
        betList = new List<int>();
        // Add a bet of 0 for each one
        for (int i = 0; i<GameManager.instance.racers.Count; i++)
        {
            betList.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
