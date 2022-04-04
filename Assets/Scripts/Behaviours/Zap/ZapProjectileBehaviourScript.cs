using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapProjectileBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject lineRender;
    
    [SerializeField] private float zapDamage = 2f;
    
    [SerializeField] private float zapDelay = 0.5f;
    [SerializeField] private float zapTimer = 0f;
    
    private List<EnemyBehaviourScript> _list;
    private List<GameObject> _listOfRay;
    private Vector3 playerTarget;

    private bool doneDamage = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _list ??= new List<EnemyBehaviourScript>();
        _listOfRay ??= new List<GameObject>();
        
        _listOfRay.Add(gameObject.transform.parent.gameObject);
        
        playerTarget = FindObjectOfType<PlayerMovementBehaviour>().transform.position;
        zapTimer = zapDelay;
        doneDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!doneDamage)
        {
            if (zapTimer < Time.deltaTime)
            {
                Zap();
                doneDamage = true;
            }
            else
            {
                zapTimer -= Time.deltaTime;
            }
        }
        
    }

    private void Zap()
    {
        _list.Sort(CompareEnemyByDistance);
        for (int i = 0; i < _list.Count; i++)
        {
            var enemy = _list[i];
            enemy.ChangeCurrentHealth(-zapDamage);
            _listOfRay.Add(enemy.gameObject);
        }
        if (_listOfRay.Count > 2)
        {
            for (int i = 0; i < _listOfRay.Count-1; i++)
            {
                var first = _listOfRay[i];
                var next= _listOfRay[i+1];
                DrawLineBetween(new Vector3[] { first.transform.position, next.transform.position });
            }
        }
        
        Destroy(gameObject.transform.parent.gameObject, 1f);
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

    private void DrawLineBetween(Vector3[] pos)
    {
        var info = Instantiate(lineRender, transform.position, transform.rotation, transform);
        LineRenderer lineRenderer = info.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPositions(pos);
        lineRenderer.gameObject.SetActive(true);
    }
}
