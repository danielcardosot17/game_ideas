using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "game_ideas/Mission", order = 0)]
[System.Serializable]
public class Mission : ScriptableObject {
    public string missionName;
    public string description;
    public string starterName;
    public string enderName;
    public float timeDeadline = -1; // in seconds = 
    public Dialog dialog;
    public RewardSO[] rewards;
    public GameEventSO disabledByTimerEvent;

    private string questName;
    private MissionStarter starter;
    private MissionEnder ender;

    private float timeToEnd = -1;

    private bool isEnabled = false;
    private bool hasStarted = false;
    private bool hasEnded = false;

    public void Enable()
    {
        isEnabled = true;
    }
    public void Disable()
    {
        isEnabled = false;
    }

    public bool IsEnabled()
    {
        return isEnabled;
    }

    public MissionStarter GetMissionStarter()
    {
        return starter;
    }

    public void SetQuestName(string name)
    {
        this.questName = name;
    }

    public string GetQuestName()
    {
        return questName;
    }

    public void SetMissionStarter(MissionStarter starter)
    {
        this.starter = starter;
    }
    public MissionEnder GetMissionEnder()
    {
        return ender;
    }

    public void SetMissionEnder(MissionEnder ender)
    {
        this.ender = ender;
    }

    public void ResetMission()
    {
        hasStarted = false;
        hasEnded = false;
        isEnabled = false;
        timeToEnd = -1;
    }

    public bool HasEnded()
    {
        return hasEnded;
    }

    public void EndMission()
    {
        hasEnded = true;
        Debug.Log("The mission " + missionName + " has ended!");
    }
    public void StartMission()
    {
        hasStarted = true;
        Debug.Log("The mission " + missionName + " has started!");
    }

    public float GetTimeToEnd()
    {
        return timeToEnd;
    }
    public void SetTimeToEnd(float time)
    {
        timeToEnd = time;
    }
}
