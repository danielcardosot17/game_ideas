using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Animator animator;
    private RoguelikePlayer player;

    private Queue<string> dialogQueue;
    private bool isOpen = false;
    private Clock globalTimer;
    public bool gameHasEnded = false;

    private void Awake() {
        gameHasEnded = false;
        player = FindObjectOfType<RoguelikePlayer>();
        globalTimer = FindObjectOfType<Clock>();
        dialogQueue = new Queue<string>();
        HideDialog();
    }

    public void NextDialog()
    {
        HideDialog();
        if(dialogQueue.Count > 0)
        {
            ShowDialog();
        }
    }

    public void ShowDialog()
    {
        if(!gameHasEnded)
        {
            globalTimer.PauseTimer();
            player.DisableInputActions();
            player.EnableUIInputActions();
            // StopAllCoroutines();
            // StartCoroutine(TypeSentence(dialogQueue.Dequeue()));
            dialogText.text = dialogQueue.Dequeue();
            isOpen = true;
            animator.SetBool("isOpen",isOpen);
        }
    }

    public void HideDialog()
    {
        globalTimer.StartTimer();
        player.EnableInputActions();
        DisableInputActions();
        dialogText.text = "";
        isOpen = false;
        animator.SetBool("isOpen",isOpen);
    }

    public void DisableInputActions()
    {
        player.DisableUIInputActions();
    }

    public void EnqueueDialog(string dialog)
    {
        if(!gameHasEnded)
        {
            dialogQueue.Enqueue(dialog);
        }
    }

    public void ClearDialogQueue()
    {
        dialogQueue.Clear();
    }

    // IEnumerator TypeSentence(string sentence)
    // {
    //     dialogText.text = "";
    //     foreach (char letter in sentence.ToCharArray())
    //     {
    //         dialogText.text += letter;
    //         yield return new WaitForSeconds(0.01f);
    //     }
    // }
}
