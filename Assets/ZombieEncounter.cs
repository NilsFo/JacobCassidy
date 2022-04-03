using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ZombieEncounter : MonoBehaviour
{
    public GameObject zombiePrefab;
    private GameStateBehaviourScript gameStateBehaviourScript;

    public CultistAI myCultist;
    public int requiredCultistDeathCount;
    public bool chaser = false;

    public SpriteRenderer mySprite;

    // Start is called before the first frame update
    void Start()
    {
        mySprite.enabled = false;
        gameStateBehaviourScript = FindObjectOfType<GameStateBehaviourScript>();
        TrySpawn();
    }

    public void TrySpawn()
    {
        if (myCultist.currentState == CultistAI.CultistState.Dead)
        {
            // Not spawning, my cultist is dead anyway
            return;
        }

        if (GetCultistDeathCount() == requiredCultistDeathCount)
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
        return gameStateBehaviourScript.NumberOfDeadCultists;
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        //Debug.DrawLine(sourceNPC.transform.position, roamingOrigin);
        var position = transform.position;
        Vector3 wireOrigin = new Vector3(position.x, position.y, position.z - 1);
        Handles.DrawWireDisc(wireOrigin, Vector3.forward, 1);
        Handles.Label(wireOrigin, "S: " + requiredCultistDeathCount);
#endif
    }
}