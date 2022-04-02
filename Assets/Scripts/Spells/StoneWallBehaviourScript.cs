using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWallBehaviourScript : ISpellBehaviourScript
{
    public override void SpawnSpell(GameObject startObj, Vector2 direction)
    {
        Debug.Log(name);
    }
}
