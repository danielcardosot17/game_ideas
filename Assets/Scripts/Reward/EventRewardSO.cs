using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventRewardSO", menuName = "game_ideas/EventRewardSO", order = 0)]
public class EventRewardSO : RewardSO
{
    [SerializeField] private string eventName;
    [SerializeField] private GameEventSO onTimeEvent;
    [SerializeField] private GameEventSO notOnTimeEvent;

    public override void AddEventsToList(List<GameEventSO> list)
    {
        if(onTimeEvent != null)
        {
            if(!list.Contains(onTimeEvent))
            {
                list.Add(onTimeEvent);
            }
        }
        if(notOnTimeEvent != null)
        {
            if(!list.Contains(notOnTimeEvent))
            {
                list.Add(notOnTimeEvent);
            }
        }
    }

    public override void AddEventNamesToList(List<string> list)
    {
        if(onTimeEvent != null)
        {
            if(!list.Contains(eventName + "OnTime"))
            {
                list.Add(eventName + "OnTime");
            }
        }
        if(notOnTimeEvent != null)
        {
            if(!list.Contains(eventName + "NotOnTime"))
            {
                list.Add(eventName + "NotOnTime");
            }
        }
    }

    public override void EventReward(bool missionOnTime)
    {
        if(missionOnTime)
        {
            if(onTimeEvent != null)
            {
                onTimeEvent.Raise();
            }
        }
        else
        {
            if(notOnTimeEvent != null)
            {
                notOnTimeEvent.Raise();
            }
        }
    }

    public override int MoneyReward(bool missionOnTime)
    {
        return 0;
    }

    public override int ReputationReward(bool missionOnTime)
    {
        return 0;
    }

    public override string DialogReward(bool missionOnTime)
    {
        return "";
    }
}