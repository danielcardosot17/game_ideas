using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestCreatorSO", menuName = "game_ideas/QuestCreatorSO", order = 0)]
public class QuestCreatorSO : ScriptableObject {
    public Quest[] firstQuests;
    public List<Quest> quests;
}