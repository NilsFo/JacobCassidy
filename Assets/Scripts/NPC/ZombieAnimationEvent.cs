using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationEvent : MonoBehaviour {
    public ZombieAI myZombie;
    
    public void FishmanAttack() {
        Debug.Log("Fishman attacks");
        myZombie.MakeAttack();
    }

    public void FishmanAttackEnd() {
        myZombie.EndAttack();
    }

    public void FishmanDespawn() {
        myZombie.DespawnAfterDeath();
    }
}
