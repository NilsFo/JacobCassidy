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
    
    [SerializeField] private GameObject spellPref;
    
    [SerializeField] private float speed = 10f;
    [SerializeField] private float flyTime = 3f;
    
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
                SpawnSpell(startObj, direction);
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
        GameObject instBullet = Instantiate(spellPref, startObj.transform.position, Quaternion.Euler(0,0,Mathf.Rad2Deg*Mathf.Atan2(direction.y, direction.x)));
        Rigidbody2D instBulletRB = instBullet.GetComponent<Rigidbody2D>();
                
        instBulletRB.AddForce(direction * speed, ForceMode2D.Force);

        Destroy(instBullet, flyTime);
    }
    
    public abstract string GetName();
}
