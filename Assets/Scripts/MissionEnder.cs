using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEnder : MonoBehaviour
{
    private Queue<Mission> missionsToEnd = new Queue<Mission>();

    public void EnqueueMission(Mission mission)
    {
        missionsToEnd.Enqueue(mission);
    }
    public Mission DequeueMission()
    {
        return missionsToEnd.Dequeue();
    }

    public int GetMissionCount()
    {
        return missionsToEnd.Count;
    }
    public Mission GetMission()
    {
        if(missionsToEnd.Count > 0)
        {
            return missionsToEnd.Peek();
        }
        else
        {
            return null;
        }
    }
}