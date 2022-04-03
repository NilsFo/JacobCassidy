using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EnemyState {
    Spawned,
    Alive,
    Dead
}

public class EnemyBehaviourScript : MonoBehaviour
{

    [SerializeField] private float maxHealth = 2;
    [SerializeField] private float currentHealth = 2;

    [SerializeField] private float deathDelay = 1f;
    [SerializeField] private float deathTimer = 0f;

    [SerializeField] private EnemyState enemyState;
    
    [SerializeField] private float damageOnContact = 1;

    public UnityEvent onDeath;
    public UnityEvent onDamageTaken;
    public delegate void OnDeathDelegate(GameObject self);
    public delegate void OnDamageDelegate(GameObject self, float damage);
    
    public event OnDeathDelegate OnDeath;
    public event OnDamageDelegate OnDamageTaken;
    

    public EnemyState EnemyState => enemyState;

    // Start is called before the first frame update
    void Start()
    {
        ResetEnemy();
        EnemieStateBehaviourScript.AddEnemy(gameObject);
        
        if (onDeath == null)
        {
            onDeath = new UnityEvent();
        }
        if (onDamageTaken == null) {
            onDamageTaken = new UnityEvent();
        }
    }

    private void OnDisable()
    {
        EnemieStateBehaviourScript.RemoveEnemy(gameObject);
    }

    private void Update()
    {
        if (enemyState == EnemyState.Spawned)
        {
            enemyState = EnemyState.Alive;
        }
        else if (enemyState == EnemyState.Dead)
        {
            if (deathTimer < 0)
            {
                //Destroy(gameObject);
            }
            else
            {
                deathTimer -= Time.deltaTime;
            }
        }
    }

    public void ResetEnemy()
    {
        currentHealth = maxHealth;
        enemyState = EnemyState.Spawned;
    }

    public void KillEnemy()
    {
        enemyState = EnemyState.Dead;
        deathTimer = deathDelay;
        onDeath.Invoke();
        OnDeath?.Invoke(gameObject);
    }
    
    public bool ChangeCurrentHealth(float value)
    {
        if (value == 0)
        {
            return true;
        }
        currentHealth += value;
        if (value < 0) {
            onDamageTaken?.Invoke();
            OnDamageTaken?.Invoke(gameObject, value);
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            KillEnemy();
            return false;
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        return true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerBehaviourScript playerBehaviourScript = other.gameObject.GetComponent<PlayerBehaviourScript>();
        if (playerBehaviourScript)
        {
            playerBehaviourScript.PlayerStateBehaviourScript.ChangeCurrentHealth(-damageOnContact);
        }
    }
}
