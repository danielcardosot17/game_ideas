using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionStarter : MonoBehaviour
{
    private Queue<Mission> missionsToStart = new Queue<Mission>();
    private SpriteRenderer exclamationEmote;

    private void Awake() {
        exclamationEmote = transform.Find("UI/Emote").GetComponent<SpriteRenderer>();
        exclamationEmote.enabled = false;
    }

    public void EnqueueMission(Mission mission)
    {
        missionsToStart.Enqueue(mission);
        EnableExclamationEmote();
    }

    private void EnableExclamationEmote()
    {
        exclamationEmote.enabled = true;
    }

    private void DisableExclamationEmote()
    {
        exclamationEmote.enabled = false;
    }

    public Mission DequeueMission()
    {
        var mission = missionsToStart.Dequeue();
        if(missionsToStart.Count == 0)
        {
            DisableExclamationEmote();
        }
        return mission;
    }

    public int GetMissionCount()
    {
        return missionsToStart.Count;
    }

    public void RemoveDisabledByTimer(Mission mission)
    {
        missionsToStart = new Queue<Mission>(missionsToStart.Where(x => x != mission));
        if(missionsToStart.Count == 0)
        {
            DisableExclamationEmote();
        }
    }
}
