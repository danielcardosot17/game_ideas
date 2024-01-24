using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reputation : MonoBehaviour
{
    private int reputation;

    private void Awake() {
        reputation = 100;
    }

    public void AddReputation(int amount)
    {
        reputation += amount;
        if(reputation < 0)
        {
            reputation = 0;
        }
        Debug.Log("Reputation = " + reputation);
    }
    public int GetReputation()
    {
        return reputation;
    }
}
