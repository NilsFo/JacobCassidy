using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectileBehaviourScript : MonoBehaviour
{
    public GameObject fireballExplosionPref;
    
    [SerializeField] private float hitDamage = 2f;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyBehaviourScript enemyBehaviourScript = other.gameObject.GetComponent<EnemyBehaviourScript>();
        if (enemyBehaviourScript)
        {
            var contact = other.GetContact(0);
            Instantiate(fireballExplosionPref, contact.point, Quaternion.Euler(0,0,Mathf.Rad2Deg*Mathf.Atan2(contact.normal.y, contact.normal.x)));
            
            enemyBehaviourScript.ChangeCurrentHealth(-hitDamage);
        }
        Destroy(gameObject);

    }
}
