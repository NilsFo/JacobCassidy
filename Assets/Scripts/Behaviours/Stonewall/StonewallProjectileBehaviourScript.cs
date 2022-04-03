using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonewallProjectileBehaviourScript : MonoBehaviour
{
    public GameObject stonewallPref;
    
    [SerializeField] private float hitDamage = 2f;

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
            DeployWall(gameObject.transform.position);
        }
        else
        {
            projectileFlyTimer -= Time.deltaTime;
        }
    }
    
    private void DeployWall(Vector3 vec3)
    {
        Vector3 playerTarget = FindObjectOfType<PlayerMovementBehaviour>().transform.position;
        var dir = playerTarget - transform.position;
        var euler = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f);
        
        Instantiate(stonewallPref, vec3, euler);

        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyBehaviourScript enemyBehaviourScript = other.gameObject.GetComponent<EnemyBehaviourScript>();
        if (enemyBehaviourScript)
        {
            enemyBehaviourScript.ChangeCurrentHealth(-hitDamage);
        }

        var contact = other.GetContact(0);
        DeployWall(new Vector3(contact.point.x, contact.point.y, -6));
    }
}
