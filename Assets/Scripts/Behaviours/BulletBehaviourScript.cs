using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviourScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyBehaviourScript enemyBehaviourScript = other.gameObject.GetComponent<EnemyBehaviourScript>();
        if (enemyBehaviourScript)
        {
            enemyBehaviourScript.ChangeCurrentHealth(-1);
        }
        Debug.Log(other.gameObject.name);
    }
}
