using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorBehaviourScript : MonoBehaviour
{

    public List<Sprite> list;
    
    public UnityEvent onDeath;
    public UnityEvent onDamageTaken;
    
    public int maxHealth = 4;
    public int currentHealth = 4;
    
    void Start()
    {
        list ??= new List<Sprite>();

        onDeath ??= new UnityEvent();
        onDamageTaken ??= new UnityEvent();
        
        currentHealth = maxHealth;
    }
    

    public bool ChangeCurrentHealth(float value)
    {
        return true;
    }
}
