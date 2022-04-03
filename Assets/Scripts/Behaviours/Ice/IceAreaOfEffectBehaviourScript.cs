using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAreaOfEffectBehaviourScript : MonoBehaviour
{
    [SerializeField] private List<CutOutBehaviourScript> listOfCutOut;
    
    [SerializeField] private float iceDamage = 2f;
    
    [SerializeField] private float iceDelay = 0.5f;
    [SerializeField] private float iceTimer = 0f;
   
    private List<EnemyBehaviourScript> _list;
    
    void Start()
    {
        _list ??= new List<EnemyBehaviourScript>();
        listOfCutOut ??= new List<CutOutBehaviourScript>();

        iceTimer = iceDelay;
    }

    private void Update()
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
            var enemy = tempList[i];
            enemy.ChangeCurrentHealth(-iceDamage);
        }
        
        //TODO Render Area
        Destroy(gameObject.transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBehaviourScript enemyBehaviourScript = other.gameObject.GetComponentInParent<EnemyBehaviourScript>();
        if (enemyBehaviourScript)
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
