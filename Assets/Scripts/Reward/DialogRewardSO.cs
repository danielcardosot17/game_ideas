using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogRewardSO", menuName = "game_ideas/DialogRewardSO", order = 0)]
public class DialogRewardSO : RewardSO
{
    [SerializeField][TextArea(3, 10)]  private string onTimeDialog;
    [SerializeField][TextArea(3, 10)]  private string notOnTimeDialog;

    public override void AddEventNamesToList(List<string> list)
    {
    }

    public override void AddEventsToList(List<GameEventSO> list)
    {
    }

    public override string DialogReward(bool missionOnTime)
    {
        return missionOnTime ? onTimeDialog : notOnTimeDialog;
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
        return 0;
    }
}