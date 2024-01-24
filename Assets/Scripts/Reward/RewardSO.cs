using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardSO", menuName = "game_ideas/RewardSO", order = 0)]
public abstract class RewardSO : ScriptableObject {
    public abstract string DialogReward(bool missionOnTime);
    public abstract int MoneyReward(bool missionOnTime);
    public abstract void EventReward(bool missionOnTime);
    public abstract int ReputationReward(bool missionOnTime);
    public abstract void AddEventsToList(List<GameEventSO> list);
    public abstract void AddEventNamesToList(List<string> list);
}