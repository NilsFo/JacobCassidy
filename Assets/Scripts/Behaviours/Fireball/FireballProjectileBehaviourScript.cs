using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectileBehaviourScript : MonoBehaviour
{
    public GameObject fireballExplosionPref;
    
    [SerializeField] private float projectileFlyDuration = 3f;
    [SerializeField] private float projectileFlyTimer = 0f;
    
    [SerializeField] private float hitDamage = 2f;

    private void Start()
    {
        projectileFlyTimer = projectileFlyDuration;
    }

    private void Update()
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
        var contact = other.GetContact(0);
        Instantiate(fireballExplosionPref, new Vector3(contact.point.x,contact.point.y, -6), Quaternion.Euler(0,0,0));
        
        EnemyBehaviourScript enemyBehaviourScript = other.gameObject.GetComponent<EnemyBehaviourScript>();
        if (enemyBehaviourScript)
        {
            enemyBehaviourScript.ChangeCurrentHealth(-hitDamage);
        }
        
        Destroy(gameObject);
    }
}
