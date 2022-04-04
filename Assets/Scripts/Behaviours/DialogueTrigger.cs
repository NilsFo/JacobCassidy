using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Sprite img;
    public string text;
    

    public void OnTriggerEnter2D(Collider2D other) {
        var player = other.GetComponent<PlayerMovementBehaviour>();
        if (player != null) {
            var conv = FindObjectOfType<ConversationUIBehaviourScript>();
            conv.AddMsg(img, text);
            Destroy(this);
        }
    }
}
