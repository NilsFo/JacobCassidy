using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IceAreaOfEffectBehaviourScript : MonoBehaviour
{
    [SerializeField] private List<CutOutBehaviourScript> listOfCutOut;

    [SerializeField] private float iceDamage = 2f;

    [SerializeField] private float iceDelay = 0.5f;
    [SerializeField] private float iceTimer = 0f;
    public float slowDuration = 5.69f;
    public ParticleSystem particles;
    private List<EnemyBehaviourScript> _list;

    public bool fired = false;

    void Start()
    {
        _list ??= new List<EnemyBehaviourScript>();
        listOfCutOut ??= new List<CutOutBehaviourScript>();

        iceTimer = iceDelay;
        fired = false;
    }

    private void Update()
    {
        // if (!fired)
        // {
        //     if (iceTimer < Time.deltaTime)
        //     {
        //         //Ice();
        //     }
        //     else
        //     {
        //         iceTimer -= Time.deltaTime;
        //     }
        // }
    }

    private void LateUpdate()
    {
        if(!fired) Ice();
    }

    private void Ice()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.layerMask = LayerMask.GetMask(new[] {"EnemyHitbox"});
        contactFilter2D.useLayerMask = true;
        var c = GetComponent<Collider2D>();
        c.OverlapCollider(contactFilter2D, colliders);
        Debug.Log("freezing " + colliders.Count + " enemies for " + slowDuration);

        var player = FindObjectOfType<PlayerMovementBehaviour>();
        foreach (var col in colliders)
        {
            var p = col.gameObject.transform.parent;
            if (p != null)
            {
                NPCMovementAI ai = p.gameObject.GetComponent<NPCMovementAI>();
                if (ai != null)
                {
                    ai.Slow(slowDuration);
                }
            }
        }

        // Spawning particles
        int count = Random.Range(65, 75);
        particles.Emit(count);
        Destroy(gameObject.transform.parent.gameObject, 2f);
        fired = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBehaviourScript enemyBehaviourScript = other.gameObject.GetComponentInParent<EnemyBehaviourScript>();
        if (enemyBehaviourScript && !_list.Contains(enemyBehaviourScript))
        {
            _list.Add(enemyBehaviourScript);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        EnemyBehaviourScript enemyBehaviourScript = other.gameObject.GetComponentInParent<EnemyBehaviourScript>();
        if (enemyBehaviourScript)
        {
            _list.Remove(enemyBehaviourScript);
        }
    }
}