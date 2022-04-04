using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBookBehaviourScript : MonoBehaviour
{
    [SerializeField] private SpellStateBehaviourScript spellStateBehaviourScript;

    [Header("0 Fireball, 1 Stonewall, 2 Zap, 3 Ice, 4 Knock")]
    [SerializeField] private int spellId;
    
    [Header("Dialogue")]
    public Sprite img;
    public string text;

    private void Start()
    {
        spellStateBehaviourScript = FindObjectOfType<SpellStateBehaviourScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovementBehaviour playerMovementBehaviour = other.gameObject.GetComponentInParent<PlayerMovementBehaviour>();
        if (playerMovementBehaviour)
        {
            spellStateBehaviourScript.AddSpell(spellId);
            
            var conv = FindObjectOfType<ConversationUIBehaviourScript>();
            conv.AddMsg(img, text);
            
            Destroy(gameObject);
        }
    }
}
