using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SpellStateBehaviourScript : MonoBehaviour
{
    
    public UnityEvent onSpellStateChange;

    public List<ISpellBehaviourScript> spellList;

    public ISpellBehaviourScript placeHold;
    
    public int currentIndex;
    
    private List<ISpellBehaviourScript> knownSpellList;

    public bool learnAll = false;
    
    private MainInputActionsSettings input;
    
    public float swapDelay = 0.2f;
    private float swapTime;
    
    // Start is called before the first frame update
    void Start()
    {
        ResetSpellState();
        
        input = FindObjectOfType<GameStateBehaviourScript>().mainInputActions;
        
        input.Toolbar.Right.performed += RightOnPerformed;
        input.Toolbar.Left.performed += LeftOnPerformed;
        
        input.Toolbar.Slot1.performed += SlotOneOnPerformed;
        input.Toolbar.Slot2.performed += SlotTwoOnPerformed;
        input.Toolbar.Slot3.performed += Slot3ThreeOnPerformed;
        input.Toolbar.Slot4.performed += SlotFourOnPerformed;
        input.Toolbar.Slot5.performed += SlotFiveOnpPerformed;
    }

    private void OnDisable()
    {
        input.Toolbar.Right.performed -= RightOnPerformed;
        input.Toolbar.Left.performed -= LeftOnPerformed;
    }

    public void ResetSpellState()
    {
        knownSpellList ??= new List<ISpellBehaviourScript>();
        currentIndex = 0;
        swapTime = 0f;
        onSpellStateChange.Invoke();

        if (learnAll)
        {
            AddSpell(0);
            AddSpell(1);
            AddSpell(2);
            AddSpell(3);
            AddSpell(4);
        }
    }

    private void Update()
    {
        if (swapTime <= Time.deltaTime)
        {
            swapTime = 0;
        }
        else
        {
            swapTime -= Time.deltaTime;
        }
    }

    public void NextSpell()
    {
        currentIndex = GetFixedIndex(currentIndex + 1);
        onSpellStateChange.Invoke();
    }

    public void PreviousSpell()
    {
        currentIndex = GetFixedIndex(currentIndex - 1);
        onSpellStateChange.Invoke();
    }

    public ISpellBehaviourScript GetCurrentSpell()
    {
        if(knownSpellList == null) return placeHold;
        if (knownSpellList.Count <= 0) return placeHold;
        var spell = knownSpellList[currentIndex];
        if (spell)
        {
            return spell;
        }
        return placeHold;
    }
    
    public ISpellBehaviourScript GetNexSpell()
    {
        if(knownSpellList == null) return placeHold;
        if (knownSpellList.Count <= 0) return placeHold;
        var tempNextIndex = GetFixedIndex(currentIndex + 1);
        var spell = knownSpellList[tempNextIndex];
        if (spell)
        {
            return spell;
        }
        return placeHold;
    }
    
    public ISpellBehaviourScript GetPreviousSpell()
    {
        if(knownSpellList == null) return placeHold;
        if (knownSpellList.Count <= 0) return placeHold;
        var tempNextIndex = GetFixedIndex(currentIndex - 1);
        var spell = knownSpellList[tempNextIndex];
        if (spell)
        {
            return spell;
        }
        return placeHold;
    }

    public bool SetSpellIndex(int index)
    {
        currentIndex = GetFixedIndex(index);
        return true;
    }

    private int GetFixedIndex(int index)
    {
        if (knownSpellList == null) return 0;
        if (knownSpellList.Count <= 0) return 0;
        var tempNextIndex = index;
        if (tempNextIndex < 0)
        {
            tempNextIndex = knownSpellList.Count - 1;
        }
        if (tempNextIndex >= knownSpellList.Count)
        {
            tempNextIndex = 0;
        }
        return tempNextIndex;
    }
    
    public bool CastSpell(GameObject startObj, Vector2 direction)
    {
        if (knownSpellList == null) return false;
        if (knownSpellList.Count <= 0) return false;
        var spell = knownSpellList[currentIndex];
        if (spell)
        {
            return spell.CastSpell(startObj, direction);
        }
        return false;
    }
    private int spellLevel = 0;
    public bool AddSpell(int index)
    {
        knownSpellList ??= new List<ISpellBehaviourScript>();
        index = spellLevel;
        spellLevel += 1;
        if (spellList != null && spellList.Count >= spellLevel && index > -1)
        {
            knownSpellList.Add(spellList[index]);
            onSpellStateChange.Invoke();
            return true;
        }
        return false;
    }
    
    private void LeftOnPerformed(InputAction.CallbackContext obj)
    {
        if(swapTime != 0) return;
        PreviousSpell();
        swapTime = swapDelay;
    }

    private void RightOnPerformed(InputAction.CallbackContext obj)
    {
        if(swapTime != 0) return;
        NextSpell(); 
        swapTime = swapDelay;
    }
    
    private void SlotFiveOnpPerformed(InputAction.CallbackContext obj)
    {
        SetSpellIndex(4);
        swapTime = swapDelay;
    }

    private void SlotFourOnPerformed(InputAction.CallbackContext obj)
    {
        SetSpellIndex(3);
        swapTime = swapDelay;
    }

    private void Slot3ThreeOnPerformed(InputAction.CallbackContext obj)
    {
        SetSpellIndex(2);
        swapTime = swapDelay;
    }

    private void SlotTwoOnPerformed(InputAction.CallbackContext obj)
    {
        SetSpellIndex(1);
        swapTime = swapDelay;
    }

    private void SlotOneOnPerformed(InputAction.CallbackContext obj)
    {
        SetSpellIndex(0);
        swapTime = swapDelay;
    }
}
