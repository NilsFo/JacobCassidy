using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieStateBehaviourScript : MonoBehaviour
{
    // static event
    public delegate void OnRemoveEnemyDelegate(GameObject enemy);

    public static event OnRemoveEnemyDelegate onRemoveEnemy;
    
    public delegate void OnAddEnemyDelegate(GameObject enemy);

    public static event OnAddEnemyDelegate onAddEnemy;

    public static List<GameObject> enemies;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static void RemoveEnemy(GameObject enemy)
    {
        enemies?.Remove(enemy);
        onRemoveEnemy?.Invoke(enemy);
    }

    public static void AddEnemy(GameObject enemy)
    {
        enemies ??= new List<GameObject>();
        enemies.Add(enemy);
        onAddEnemy?.Invoke(enemy);
    }
}
