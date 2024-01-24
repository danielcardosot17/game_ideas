using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoneyRewardSO", menuName = "game_ideas/MoneyRewardSO", order = 0)]
public class MoneyRewardSO : RewardSO
{
    [SerializeField] private int moneyAmount;

    public override void AddEventsToList(List<GameEventSO> list)
    {
    }

    public override void AddEventNamesToList(List<string> list)
    {
    }

    public override void EventReward(bool missionOnTime)
    {
    }

    public override int MoneyReward(bool missionOnTime)
    {
        return missionOnTime ? moneyAmount : moneyAmount/2;
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