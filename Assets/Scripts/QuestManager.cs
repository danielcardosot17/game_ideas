using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestCreatorSO questCreatorSO;
    [SerializeField] private RewardManager rewardManager;
    [SerializeField] private Clock worldClock;
    private DialogManager dialogManager;
    private List<Quest> allQuests = new List<Quest>();
    private List<Mission> allMissions = new List<Mission>();
    private MissionStarter[] allMissionStarters;
    private MissionEnder[] allMissionEnders;

    private void Start() {
        dialogManager = GetComponent<DialogManager>();
        allQuests = questCreatorSO.quests;
        foreach(var quest in allQuests)
        {
            ResetQuest(quest);
            quest.missionsQueue = new Queue<Mission>();
            foreach(var mission in quest.missions)
            {
                ResetMission(mission);
                allMissions.Add(mission);
                quest.missionsQueue.Enqueue(mission);
                mission.SetQuestName(quest.questName);
            }
        }
        allMissionStarters = FindObjectsOfType<MissionStarter>();
        allMissionEnders = FindObjectsOfType<MissionEnder>();

        SetMissionsStarterAndEnder();
        rewardManager.SetEventListeners(GetAllEventRewards(), GetAllEventNames());
        EnableFirstQuests();
        EnableFirstMissions();
    }

    private void ResetMission(Mission mission)
    {
        mission.ResetMission();
    }

    private void ResetQuest(Quest quest)
    {
        quest.ResetQuest();
    }

    private List<string> GetAllEventNames()
    {
        var nameList = new List<string>();
        foreach(var mission in allMissions)
        {
            if(mission.timeDeadline > 0)
            {
                if(mission.disabledByTimerEvent != null)
                {
                    if(!nameList.Contains(mission.missionName + "DisabledByTimer"))
                    {
                        nameList.Add(mission.missionName + "DisabledByTimer");
                    }
                }
            }
            foreach(var reward in mission.rewards)
            {
                reward.AddEventNamesToList(nameList);
            }
        }
        foreach(var quest in allQuests)
        {
            foreach(var reward in quest.rewards)
            {
                reward.AddEventNamesToList(nameList);
            }
        }
        return nameList;
    }

    public void EnableQuest(string questName)
    {
        foreach(var quest in allQuests)
        {
            if(quest.questName == questName)
            {
                quest.isEnabled = true;
                EnableMission(quest.missionsQueue.Peek());
            }
        }
    }

    private List<GameEventSO> GetAllEventRewards()
    {
        var eventList = new List<GameEventSO>();
        foreach(var mission in allMissions)
        {
            if(mission.timeDeadline > 0)
            {
                if(mission.disabledByTimerEvent != null)
                {
                    if(!eventList.Contains(mission.disabledByTimerEvent))
                    {
                        eventList.Add(mission.disabledByTimerEvent);
                    }
                }
            }
            foreach(var reward in mission.rewards)
            {
                reward.AddEventsToList(eventList);
            }
        }
        foreach(var quest in allQuests)
        {
            foreach(var reward in quest.rewards)
            {
                reward.AddEventsToList(eventList);
            }
        }
        return eventList;
    }

    public string GetStatistics()
    {
        string statistics = "";
        var completedMissionsInTime = 0;
        var completedMissionsNotInTime = 0;
        var completedQuests = 0;

        foreach (var mission in allMissions)
        {
            if(mission.HasEnded())
            {
                if(WasMissionCompletedOnTime(mission))
                {
                    completedMissionsInTime++;
                }
                else
                {
                    completedMissionsNotInTime++;
                }
            }
        }
        foreach (var quest in allQuests)
        {
            if(quest.HasEnded())
            {
                completedQuests++;
            }
        }
        statistics = "Messages delivered: " + completedMissionsInTime + ". Quests completed: " + completedQuests + ". Total number of Messages and Quests: " + allMissions.Count + " messages and " + allQuests.Count + " quests";
        return statistics;
    }

    private void SetMissionsStarterAndEnder()
    {
        foreach(var mission in allMissions)
        {
            foreach(var starter in allMissionStarters)
            {
                if(starter.name == mission.starterName)
                {
                    mission.SetMissionStarter(starter);
                }
            }
            foreach(var ender in allMissionEnders)
            {
                if(ender.name == mission.enderName)
                {
                    mission.SetMissionEnder(ender);
                }
            }
        }
    }


    private void EnableFirstQuests()
    {
        foreach(var firstQuest in questCreatorSO.firstQuests)
        {
            foreach(var quest in allQuests)
            {
                if(quest == firstQuest)
                {
                    quest.isEnabled = true;
                }
            }
        }
    }
    private void EnableFirstMissions()
    {
        foreach(var quest in allQuests)
        {
            if(quest.isEnabled)
            {
                var firstMission = quest.missionsQueue.Peek();
                EnableMission(firstMission);
            }
        }
    }

    public void EndMission(Mission mission)
    {
        dialogManager.EnqueueDialog(
            "Delivered message from " + mission.starterName + "."
        );
        mission.SetTimeToEnd(worldClock.GetGlobalTimer());
        mission.EndMission();
        DisableMission(mission);
        rewardManager.GetRewards(WasMissionCompletedOnTime(mission), mission.rewards);
        var quest = GetQuestFromName(mission.GetQuestName());
        quest.missionsQueue.Dequeue();
        if(quest.missionsQueue.Count > 0)
        {
            EnableMission(quest.missionsQueue.Peek());
        }
        else
        {
            EndQuest(quest);
            rewardManager.GetRewards(WasMissionCompletedOnTime(mission), quest.rewards);
        }
        dialogManager.ShowDialog();
    }

    private bool WasMissionCompletedOnTime(Mission mission)
    {
        if(mission.timeDeadline <= 0)
        {
            return true;
        }
        return mission.GetTimeToEnd() <= mission.timeDeadline;
    }

    private void EndQuest(Quest quest)
    {
        quest.EndQuest();
        quest.isEnabled = false;
    }

    private void DisableMission(Mission mission)
    {
        mission.Disable();
    }

    public void StartMission(Mission mission)
    {
        var timeDeadlineString = "";
        if(mission.timeDeadline > 0)
        {
            var time = TimeSpan.FromSeconds(mission.timeDeadline);
            timeDeadlineString = mission.dialog.timeHint + " " + time.ToString("mm\\:ss") + ". ";
        }
        else
        {
            timeDeadlineString = "";
        }
        dialogManager.EnqueueDialog(
            mission.dialog.introduction + " " + // Hello, Courier! Please send this to
            mission.enderName + ". " + mission.enderName + " " + 
            mission.dialog.location + ". " + 
            timeDeadlineString + 
            mission.dialog.plotHint
            );
        mission.GetMissionEnder().EnqueueMission(mission);
        mission.StartMission();
        var quest = GetQuestFromName(mission.GetQuestName());
        quest.StartQuest();
        dialogManager.ShowDialog();
    }

    private Quest GetQuestFromName(string name)
    {
        return allQuests.Find(quest => quest.questName == name);
    }

    public void EnableMission(Mission mission)
    {
        mission.Enable();
        mission.GetMissionStarter().EnqueueMission(mission);
        if(mission.timeDeadline > 0)
        {
            if(mission.disabledByTimerEvent != null)
            {
                worldClock.SetEventTrigger(mission.timeDeadline, mission);
            }
        }
    }
}
