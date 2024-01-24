using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    [SerializeField][TextArea(3, 10)] private string startGameDialog1;
    [SerializeField][TextArea(3, 10)] private string startGameDialog2;
    [SerializeField][TextArea(3, 10)] private string startGameDialog3;
    [SerializeField] private  Sound ending1music;
    [SerializeField] private  Sound ending2music;
    [SerializeField] private  float fadeoutTime = 1;
    [SerializeField] private Transform wife;
    [SerializeField] private Vector3 ending1Position;
    [SerializeField] private Vector3 ending2Position;
    [SerializeField][TextArea(3, 10)] private string ending1Text;
    [SerializeField][TextArea(3, 10)] private string ending2Text;
    [SerializeField] private AudioManagerSO audioManagerSO;
    private  Image fadeoutImage;
    private MusicMaster musicMaster;
    private RoguelikePlayer player;
    private Animator endingDialogAnimator;
    private TMP_Text endingDialogText;
    private DialogManager dialogManager;
    private QuestManager questManager;
    private bool hasEnded = false;
    private Clock globalTimer;
    private Money money;

    private void Start() {
        musicMaster = FindObjectOfType<MusicMaster>();
        player = FindObjectOfType<RoguelikePlayer>();
        endingDialogAnimator = GetComponentInChildren<Animator>();
        endingDialogText = GetComponentInChildren<TMP_Text>();
        dialogManager = GetComponent<DialogManager>();
        questManager = GetComponent<QuestManager>();
        fadeoutImage = GetComponentInChildren<Image>();
        globalTimer = FindObjectOfType<Clock>();
        money = FindObjectOfType<Money>();
        endingDialogAnimator.SetBool("isOpen",false);
        endingDialogText.text = "";
        StartGame();
    }

    private void StartGame()
    {
        dialogManager.EnqueueDialog(startGameDialog1 + money.GetEndGameMoneyAmount() + startGameDialog2 + globalTimer.GetTotalTimeString() + startGameDialog3);
        dialogManager.ShowDialog();
    }

    public void EndGameByTime()
    {
        if(!hasEnded)
        {
            SetUpEndGame();
            musicMaster.StopMusic();
            musicMaster.PlayMusic(ending1music);
            StartCoroutine(FadeOut(ending1Position, ending1Text));
        }
    }

    public void EndGameByMoney()
    {
        if(!hasEnded)
        {
            SetUpEndGame();
            musicMaster.StopMusic();
            musicMaster.PlayMusic(ending2music);
            StartCoroutine(FadeOut(ending2Position, ending2Text));
        }
    }

    private void SetUpPositionsEnding(Vector3 endingPosition)
    {
        player.transform.position = endingPosition;
        wife.position = endingPosition + new Vector3(-2,0,0);
    }

    IEnumerator  FadeIn()
    {
        for (float i = fadeoutTime; i >= 0; i -= Time.deltaTime)
        {
            fadeoutImage.color = new Color(0, 0, 0, i/fadeoutTime);
            yield return null;
        }
    }
    IEnumerator  FadeOut(Vector3 endingPosition, string endingText)
    {
        for (float i = 0; i <= fadeoutTime; i += Time.deltaTime)
        {
            fadeoutImage.color = new Color(0, 0, 0, i/fadeoutTime);
            yield return null;
        }   
        SetUpPositionsEnding(endingPosition);
        endingDialogText.text =  endingText + questManager.GetStatistics();
        endingDialogAnimator.SetBool("isOpen",true);
        StartCoroutine(FadeIn());
    }

    private void SetUpEndGame()
    {
        hasEnded = true;
        dialogManager.gameHasEnded = true;
        dialogManager.ClearDialogQueue();
        dialogManager.HideDialog();
        dialogManager.DisableInputActions();
        player.DisableInputActions();
        globalTimer.PauseTimer();
        audioManagerSO.Play("boat_bell", player.transform.position);
    }
}
