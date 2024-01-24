using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [TextArea(3, 10)] public string introduction = "Hello, Courier! Please send this message to ";
    [TextArea(3, 10)] public string location = "is in the South Woods";
    [TextArea(3, 10)] public string timeHint = "He needs to receive this before";
    [TextArea(3, 10)] public string plotHint = "His life is in danger!";
}
