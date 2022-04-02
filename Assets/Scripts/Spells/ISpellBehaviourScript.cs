using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISpellBehaviourScript : MonoBehaviour
{
    [SerializeField] private PlayerStateBehaviourScript playerStateBehaviourScript;

    [SerializeField] private Sprite sprite;
    
    [SerializeField] private float spellCooldown = 1f;
    [SerializeField] private float spellTimer = 0f;

    [SerializeField] private float spellCost = 5f;
    
    private void Update()
    {
        if (spellTimer <= Time.deltaTime)
        {
            spellTimer = 0;
        }
        else
        {
            spellTimer -= Time.deltaTime;
        }
    }
    
    private void Awake()
    {
        Debug.Assert(playerStateBehaviourScript != null, gameObject);
    }
    
    public bool CastSpell(GameObject startObj, Vector2 direction)
    {
        if (spellTimer == 0 && IsCastable())
        {
            if (playerStateBehaviourScript.ChangeCurrentSanity(-spellCost))
            {
                Debug.Log("Cast: Fireball");
                return true;
            }
        }
        return false;
    }

    public bool IsCastable()
    {
        if (playerStateBehaviourScript.CurrentSanity >= spellCost) return true;
        return false;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public abstract void SpawnSpell(GameObject startObj, Vector2 direction);
}
