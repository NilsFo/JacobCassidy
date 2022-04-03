using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackProjectileBehaviourScript : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 200f;
    
    [SerializeField] private float projectileFlyDuration = 0.5f;
    [SerializeField] private float projectileFlyTimer = 0f;

    void Start()
    {
        projectileFlyTimer = projectileFlyDuration;
    }
    
    void Update()
    {
        if (projectileFlyTimer < Time.deltaTime)
        {
            Destroy(gameObject);
        }
        else
        {
            projectileFlyTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyBehaviourScript enemyBehaviourScript = other.gameObject.GetComponent<EnemyBehaviourScript>();
        if (enemyBehaviourScript)
        {
            //enemyBehaviourScript.Add
            //TODO Knockback
        }
        Destroy(gameObject);
    }
}
