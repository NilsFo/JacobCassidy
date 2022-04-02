using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellStateBehaviourScript : MonoBehaviour
{
    
    public UnityEvent onSpellStateChange;

    public List<ISpellBehaviourScript> spellList;

    public ISpellBehaviourScript placeHold;
    
    public int currentIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        ResetSpellState();
    }

    public void ResetSpellState()
    {
        currentIndex = 0;
        onSpellStateChange.Invoke();
    }

    public void NextSpell()
    {
        currentIndex++;
        if (currentIndex >= spellList.Count)
        {
            currentIndex = 0;
        }
        onSpellStateChange.Invoke();
    }

    public void PreviousSpell()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = spellList.Count - 1;
        }
        onSpellStateChange.Invoke();
    }

    public ISpellBehaviourScript GetCurrentSpell()
    {
        if (spellList.Count <= 0) return placeHold;
        var spell = spellList[currentIndex];
        if (spell)
        {
            return spell;
        }
        return placeHold;
    }
    
    public ISpellBehaviourScript GetNexSpell()
    {
        if (spellList.Count <= 0) return placeHold;
        var tempNextIndex = currentIndex + 1;
        if (tempNextIndex >= spellList.Count)
        {
            tempNextIndex = 0;
        }
        var spell = spellList[tempNextIndex];
        if (spell)
        {
            return spell;
        }
        return placeHold;
    }
    
    public ISpellBehaviourScript GetPreviousSpell()
    {
        if (spellList.Count <= 0) return placeHold;
        var tempNextIndex = currentIndex - 1;
        if (tempNextIndex < 0)
        {
            tempNextIndex = spellList.Count - 1;
        }
        if (tempNextIndex < 0) return placeHold;
        var spell = spellList[tempNextIndex];
        if (spell)
        {
            return spell;
        }
        return placeHold;
    }

    public bool CastSpell(GameObject startObj, Vector2 direction)
    {
        if (spellList.Count < 0) return false;
        var spell = spellList[currentIndex];
        if (spell)
        {
            return spell.CastSpell(startObj, direction);
        }
        return false;
    }

    public bool AddSpell(ISpellBehaviourScript script)
    {
        spellList ??= new List<ISpellBehaviourScript>();
        spellList.Add(script);
        onSpellStateChange.Invoke();
        return true;
    }
}
