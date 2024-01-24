using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReputationRewardSO", menuName = "game_ideas/ReputationRewardSO", order = 0)]
public class ReputationRewardSO : RewardSO
{
    [SerializeField] private int reputationAmount;
    public override void AddEventNamesToList(List<string> list)
    {
    }

    public override void AddEventsToList(List<GameEventSO> list)
    {
    }

    public override string DialogReward(bool missionOnTime)
    {
        return "";
    }

    public override void EventReward(bool missionOnTime)
    {
    }

    public override int MoneyReward(bool missionOnTime)
    {
        return 0;
    }

    public override int ReputationReward(bool missionOnTime)
    {
        return missionOnTime ? reputationAmount : (-1 * reputationAmount);
    }
}