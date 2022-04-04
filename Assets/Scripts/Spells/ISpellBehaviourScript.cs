using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ISpellBehaviourScript : MonoBehaviour
{
    [SerializeField] private PlayerStateBehaviourScript playerStateBehaviourScript;
    [SerializeField] private SpellStateBehaviourScript spellStateBehaviourScript;
    
    [SerializeField] private Sprite sprite;
    
    [SerializeField] private float spellCooldown = 1f;
    [SerializeField] private float spellTimer = 0f;

    [SerializeField] private float spellCost = 5f;
    
    [SerializeField] private GameObject spellPref;
    
    [SerializeField] private float speed = 10f;

    private void Start()
    {
        spellTimer = 0f;
    }

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
        Debug.Assert(spellStateBehaviourScript != null, gameObject);
    }
    
    public bool CastSpell(GameObject startObj, Vector2 direction)
    {
        if (spellTimer == 0 && IsCastable())
        {
            if (playerStateBehaviourScript.ChangeCurrentSanity(-spellCost))
            {
                SpawnSpell(startObj, direction);
                spellTimer = spellCooldown;
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

    public void SpawnSpell(GameObject startObj, Vector2 direction)
    {
        var normal = direction.normalized;
        normal = normal * 0.5f;
        GameObject instBullet = Instantiate(spellPref, new Vector3(startObj.transform.position.x+normal.x, startObj.transform.position.y+normal.y, -6), Quaternion.Euler(0,0,Mathf.Rad2Deg*Mathf.Atan2(direction.y, direction.x)));
        Rigidbody2D instBulletRB = instBullet.GetComponent<Rigidbody2D>();
        if(instBulletRB != null)
            instBulletRB.AddForce(direction * speed, ForceMode2D.Force);
    }
    
    public abstract string GetName();
}
