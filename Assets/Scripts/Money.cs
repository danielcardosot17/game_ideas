using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private GameEventSO endGameByMoneyEvent;
    [SerializeField] private int endGameMoneyAmount;
    private int money;

    private void Awake() {
        money = 0;
        DisplayMoney();
    }

    private void DisplayMoney()
    {
        moneyText.text = money.ToString("D2");
    }

    public void AddMoney(int amount)
    {
        money += amount;
        if(money < 0)
        {
            money = 0;
        }
        DisplayMoney();
        if(money >= endGameMoneyAmount)
        {
            endGameByMoneyEvent.Raise();
        }
        Debug.Log("money = " + money);
    }

    public string GetEndGameMoneyAmount()
    {
        return endGameMoneyAmount.ToString();
    }
    public int GetMoney()
    {
        return money;
    }
}
