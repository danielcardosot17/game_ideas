using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    private MusicMaster musicMaster;
    private void Start() {
        musicMaster = transform.root.GetComponent<MusicMaster>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
       musicMaster.PlayNextMusic(); 
    }

}
