using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasicUnit : MonoBehaviour
{
    private TMP_Text nameText;
    // Start is called before the first frame update
    void Awake()
    {
        nameText = GetComponentInChildren<TMP_Text>();
        nameText.text = gameObject.name;
    }
}
