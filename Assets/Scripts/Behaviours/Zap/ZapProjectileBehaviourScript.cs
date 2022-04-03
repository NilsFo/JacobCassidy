using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapProjectileBehaviourScript : MonoBehaviour
{
 
    [SerializeField] private float zapDamage = 2f;
    
    [SerializeField] private float zapDelay = 0.5f;
    [SerializeField] private float zapTimer = 0f;
    
    private List<EnemyBehaviourScript> _list;
    private Vector3 playerTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        _list ??= new List<EnemyBehaviourScript>();
        playerTarget = FindObjectOfType<PlayerMovementBehaviour>().transform.position;
        zapTimer = zapDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (zapTimer < Time.deltaTime)
        {
            Zap();
        }
        else
        {
            zapTimer -= Time.deltaTime;
        }
    }

    private void Zap()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            var enemy = _list[i];
            enemy.ChangeCurrentHealth(-zapDamage);
        }
        _list.Sort(CompareEnemyByDistance);
        //TODO Render Lines
        Destroy(gameObject.transform.parent.gameObject);
    }

    private int CompareEnemyByDistance(EnemyBehaviourScript x, EnemyBehaviourScript y)
    {
        if (x == null)
        {
            if (y == null)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            // If x is not null...
            //
            if (y == null)
                // ...and y is null, x is greater.
            {
                return 1;
            }
            else
            {
                // ...and y is not null, compare the
                // lengths of the two strings.
                //
                var dirX = playerTarget - x.gameObject.transform.position;
                var dirY = playerTarget - y.gameObject.transform.position;
                int retval = dirX.magnitude.CompareTo(dirY.magnitude);
                if (retval != 0)
                {
                    // If the distance are not of equal length,
                    // the longer distance is greater.
                    return retval;
                }
                else
                {
                    // If the strings are of equal length,
                    // sort them with ordinary string comparison.
                    return 1;
                }
            }
        }
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
