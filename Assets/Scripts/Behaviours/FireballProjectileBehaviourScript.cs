using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectileBehaviourScript : MonoBehaviour
{
    public static GameObject FireballExplosion;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyBehaviourScript enemyBehaviourScript = other.gameObject.GetComponent<EnemyBehaviourScript>();
        if (enemyBehaviourScript)
        {
            
        }
        Destroy(gameObject);

    }
}
