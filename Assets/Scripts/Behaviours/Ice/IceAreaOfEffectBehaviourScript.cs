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
        if (!fired)
        {
            if (iceTimer < Time.deltaTime)
            {
                Ice();
            }
            else
            {
                iceTimer -= Time.deltaTime;
            }
        }
        
    }

    private void Ice()
    {
        var tempList = _list;
        foreach (var cutout in listOfCutOut)
        {
            var listCutOut = cutout.List;
            var newList = new List<EnemyBehaviourScript>();
            foreach (var enemy in tempList)
            {
                if (!listCutOut.Contains(enemy))
                {
                    newList.Add(enemy);
                }
            }
            tempList = newList;
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            var enemy = tempList[i].gameObject;
            NPCMovementAI npcMovementAI = enemy.GetComponent<NPCMovementAI>();
            if (npcMovementAI != null)
            {
                Debug.Log(tempList[i].name + "Slow !!!");
                npcMovementAI.Slow(slowDuration);
            }
        }
        
        int count = Random.Range(65, 75);
        particles.Emit(count);
        Destroy(gameObject.transform.parent.gameObject);
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
