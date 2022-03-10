using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Player;

public class moneyMenu : MonoBehaviour
{
    //TODO add to display class or menuhandler class
    public PlayerStats Money;

    public void Start()
    {
        Money = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }
    
    private void OnEnable()
    {
        if(Money)
        GetComponent<TextMeshProUGUI>().text = PlayerStats.StatTypeToPlayerStat[StatType.Money].getAmount().ToString();
    }
}
