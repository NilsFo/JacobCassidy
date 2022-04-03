using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEncounter : MonoBehaviour
{
    public GameObject zombiePrefab;

    public CultistAI myCultist;
    public int requiredCultistDeathCount;
    public bool chaser = false;

    // Start is called before the first frame update
    void Start()
    {
        TrySpawn();
    }

    public void TrySpawn()
    {
        if (myCultist.currentState == CultistAI.CultistState.Dead)
        {
            // Not spawning, my cultist is dead anyway
            return;
        }

        if (GetCultistDeathCount() < requiredCultistDeathCount)
        {
            // Not enough cultists are dead. Not spawning.
            return;
        }

        GameObject newZombie = Instantiate(zombiePrefab, new Vector3(transform.position.x, transform.position.y, 0),
            Quaternion.identity);
        ZombieAI zombieAI = newZombie.GetComponent<ZombieAI>();

        zombieAI.currentState = ZombieAI.ZombieState.Roaming;
        if (chaser)
        {
            zombieAI.currentState = ZombieAI.ZombieState.GoToLastKnownLocation;
        }
    }

    public int GetCultistDeathCount()
    {
        // TODO implement
        return 1;
    }
}