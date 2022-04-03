using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectileBehaviourScript : MonoBehaviour
{
    public GameObject fireballExplosionPref;
    
    [SerializeField] private float hitDamage = 2f;
    
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
