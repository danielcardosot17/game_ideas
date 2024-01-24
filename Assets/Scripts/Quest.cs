using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "game_ideas/Quest", order = 0)]
[System.Serializable]
public class Quest : ScriptableObject {
    public string questName;
    public string description;
    public bool isEnabled; // if true, starts with this quest open
    public List<Mission> missions;
    public RewardSO[] rewards;


    public Queue<Mission> missionsQueue;
    private bool hasStarted = false;
    private bool hasEnded = false;

    public void StartQuest()
    {
        hasStarted = true;
    }

    public void EndQuest()
    {
        hasEnded = true;
    }
    public bool HasEnded()
    {
        return hasEnded;
    }

    public void ResetQuest()
    {
        isEnabled = false;
        hasStarted = false;
        hasEnded = false;
    }
}
