using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPlaceholderBehaviourScript : ISpellBehaviourScript
{
    public override void SpawnSpell(GameObject startObj, Vector2 direction)
    {
        Debug.Log("Nothing to see here!", gameObject);
    }
}
