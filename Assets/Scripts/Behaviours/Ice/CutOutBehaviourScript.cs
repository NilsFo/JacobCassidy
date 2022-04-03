using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutOutBehaviourScript : MonoBehaviour
{
    
    private List<EnemyBehaviourScript> _list;
    
    void Start()
    {
        _list ??= new List<EnemyBehaviourScript>();
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

    public List<EnemyBehaviourScript> List => _list;
}
