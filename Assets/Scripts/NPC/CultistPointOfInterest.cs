using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistPointOfInterest : MonoBehaviour
{
    public List<GameObject> congregation;
    public int congregationSize;


    public void AddZombie(GameObject newZombie)
    {
        EnemyBehaviourScript script = newZombie.GetComponent<EnemyBehaviourScript>();
        congregation.Add(script.gameObject);
        script.OnDeath += ZombieOnDeath;
    }

    private void ZombieOnDeath(GameObject zombie)
    {
        congregation.Remove(zombie);
    }

    public bool NeedsZombie()
    {
        return congregation.Count < congregationSize;
    }
    
}