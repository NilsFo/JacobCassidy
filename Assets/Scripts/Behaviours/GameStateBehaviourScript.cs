using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateBehaviourScript : MonoBehaviour
{

    public UnityEvent onBeforeRestart;
    
    //Player Data
    [SerializeField] private float playerMaxHealth;
    [SerializeField] private float playerCurrentHealth;
    
    [SerializeField] private float playerMaxSanity;
    [SerializeField] private float playerCurrentSanity;
    
    [SerializeField] private float playerMaxAmmo;
    [SerializeField] private float playerCurrentAmmo;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
