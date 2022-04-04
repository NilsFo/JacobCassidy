using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplosionBehaviourScript : MonoBehaviour
{

    [SerializeField] private float explosionDamage = 1f;

    [SerializeField] private float stayDelay = 1f;
    [SerializeField] private float explosionDelay = 1f;
    [SerializeField] private float explosionTimer = 0f;

    public ParticleDelayedRemover delayedRemover;
    public ParticleSystem particleSystem;
    public int particleBurstCount = 95;

    private List<EnemyBehaviourScript> _list;

    public GameObject parent;
    
    // Start is called before the first frame update
    void Start()
    {
        explosionTimer = explosionDelay;
        particleSystem.Emit(particleBurstCount);
        _list ??= new List<EnemyBehaviourScript>();
    }

    private void Update()
    {
        if (explosionTimer < Time.deltaTime)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                var enemy = _list[i];
                enemy.ChangeCurrentHealth(-explosionDamage);
            }
            Destroy(parent, stayDelay);
        }
        else
        {
            explosionTimer -= Time.deltaTime;
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBehaviourScript enemyBehaviourScript = other.gameObject.GetComponentInParent<EnemyBehaviourScript>();
        if (enemyBehaviourScript)
        {
            _list ??= new List<EnemyBehaviourScript>();
            _list.Add(enemyBehaviourScript);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        EnemyBehaviourScript enemyBehaviourScript = other.gameObject.GetComponentInParent<EnemyBehaviourScript>();
        if (enemyBehaviourScript)
        {
            _list ??= new List<EnemyBehaviourScript>();
            _list.Remove(enemyBehaviourScript);
        }
    }
}
