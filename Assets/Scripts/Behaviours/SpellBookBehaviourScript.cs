using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBookBehaviourScript : MonoBehaviour
{
    [SerializeField] private SpellStateBehaviourScript spellStateBehaviourScript;

    [SerializeField] private int spellId;
    
    private void Awake()
    {
        Debug.Assert(spellStateBehaviourScript != null, gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovementBehaviour playerMovementBehaviour = other.gameObject.GetComponentInParent<PlayerMovementBehaviour>();
        if (playerMovementBehaviour)
        {
            spellStateBehaviourScript.AddSpell(spellId);
            Destroy(gameObject);
        }
    }
}
