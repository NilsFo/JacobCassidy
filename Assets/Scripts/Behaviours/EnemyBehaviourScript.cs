using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    // Start is called before the first frame update
    void Start()
    {
        ResetEnemy();
        EnemieStateBehaviourScript.AddEnemy(gameObject);
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
                Destroy(gameObject);
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
    }
    
    public bool ChangeCurrentHealth(float value)
    {
        if (value == 0)
        {
            return true;
        }
        currentHealth += value;
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
}
