using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private Money playerMoney;
    [SerializeField] private Reputation playerReputation;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private QuestManager questManager;

    public void GetRewards(bool missionOnTime , RewardSO[] rewards)
    {
        foreach(var reward in rewards)
        {
            if(!String.IsNullOrEmpty(reward.DialogReward(missionOnTime)))
            {
                dialogManager.EnqueueDialog(reward.DialogReward(missionOnTime));
            }

            playerMoney.AddMoney(reward.MoneyReward(missionOnTime));

            playerReputation.AddReputation(reward.ReputationReward(missionOnTime));

            reward.EventReward(missionOnTime);
        }
    }

    public void SetEventListeners(List<GameEventSO> rewardEvents, List<string> nameList)
    {
        var listenerList = new List<GameEventListener>();
        foreach(var rewardEvent in rewardEvents)
        {
            GameEventListener listener = gameObject.AddComponent<GameEventListener>() as GameEventListener;
            listener.Event = rewardEvent;
            listener.Event.RegisterListener(listener);
            listener.Response = new UnityEvent();
            listenerList.Add(listener);
        }
        for (int i = 0; i < listenerList.Count; i++)
        {
            UnityAction unityAction = stringFunctionToUnityAction(this, nameList[i]);
            listenerList[i].Response.AddListener(unityAction);
        }
    }
    UnityAction stringFunctionToUnityAction(object target, string functionName)
    {
        UnityAction action = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), target, functionName);
        return action;
    }

////////////////////////////// EVENTS /////////////////////////////////////////

////////////////// The Cook Quest//////////////////////////

    private int ingredients = 0;
    public void FirstIngredientQuestOnTime()
    {
        questManager.EnableQuest("FirstIngredientQuest");
    }
    
    public void SecondIngredientQuestOnTime()
    {
        questManager.EnableQuest("SecondIngredientQuest");
    }

    public void ThirdIngredientQuestOnTime()
    {
        questManager.EnableQuest("ThirdIngredientQuest");
    }
    public void IngredientCompleteOnTime()
    {
        ingredients++;
        if(ingredients == 3)
        {
            questManager.EnableQuest("DeliverFoodQuest");
        }
    }

////////////////// The Shipment Quest//////////////////////////


    private int lighthouseParts = 0;
    public void FirstPartQuestOnTime()
    {
        questManager.EnableQuest("FirstPartQuest");
    }
    
    public void SecondPartQuestOnTime()
    {
        questManager.EnableQuest("SecondPartQuest");
    }

    public void ThirdPartQuestOnTime()
    {
        questManager.EnableQuest("ThirdPartQuest");
    }
    public void PartCompleteOnTime()
    {
        lighthouseParts++;
        if(lighthouseParts == 3)
        {
            questManager.EnableQuest("DeliverShipmentQuest");
        }
    }


////////////////// QueenLoveAffairQuest //////////////////////////


    [SerializeField] private GameObject queenNaplir;
    [SerializeField] private GameObject guardLeonard;
    public void QueenLoveAffairOnTime()
    {
        KillTheQueenAndLeonard();
    }

    private void KillTheQueenAndLeonard()
    {
        queenNaplir.SetActive(false);
        guardLeonard.SetActive(false);
    }
}
