using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float gameTotalTimeInSeconds = 1200; // 20 * 60s
    [SerializeField] private GameEventSO endGameByTimeEvent;

    private float globalTimer = 0.0f;
    private bool isTimer = false;
    
    private void Update()
    {
        if(isTimer)
        {
            globalTimer += Time.deltaTime;
            DisplayTime();
            if(globalTimer >= gameTotalTimeInSeconds)
            {
                isTimer = false;
                endGameByTimeEvent.Raise(); // End game event
            }
        }
    }

    public string GetTotalTimeString()
    {
        return TimeSpan.FromSeconds(gameTotalTimeInSeconds).ToString("mm\\:ss");
    }
    private void DisplayTime()
    {
        timerText.text = TimeSpan.FromSeconds(globalTimer).ToString("mm\\:ss");
    }
    public void StartTimer()
    {
        isTimer = true;
    }
    public void PauseTimer()
    {
        isTimer = false;
    }

    public float GetGlobalTimer()
    {
        return globalTimer;
    }

    public void SetEventTrigger(float triggerTime, Mission mission)
    {
        StartCoroutine(SetTrigger(triggerTime, mission));
    }

    private IEnumerator SetTrigger(float triggerTime, Mission mission)
    {
        float timer = 0.0f;
        while(timer <= triggerTime)
        {
            if(isTimer)
            {
                timer += Time.deltaTime;
            }
            yield return null;
        }
        if(!mission.HasEnded())
        {
            mission.Disable();
            mission.GetMissionStarter().RemoveDisabledByTimer(mission);
            mission.disabledByTimerEvent.Raise();
        }
    }
}
