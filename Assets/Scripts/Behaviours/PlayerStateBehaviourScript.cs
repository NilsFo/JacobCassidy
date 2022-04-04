using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerStateBehaviourScript : MonoBehaviour
{
    //Settings
    [SerializeField] private float chargeStaminaPerSec = 0.5f;
    [SerializeField] private float reloadDelay = 0.5f;

    //Player Data
    [SerializeField] private float maxHealth = 20;
    [SerializeField] private float currentHealth = 20;
    
    [SerializeField] private float maxSanity = 20;
    [SerializeField] private float currentSanity = 20;
    
    [SerializeField] private float maxAmmo = 5;
    [SerializeField] private float currentAmmo = 5;
    
    [SerializeField] private float maxStamina = 1;
    [SerializeField] private float currentStamina = 1;
    
    //Player Events
    public UnityEvent onCurrentHealthChange;
    public UnityEvent onCurrentSanityChange;
    public UnityEvent onCurrentAmmoChange;
    public UnityEvent onCurrentStaminaChange;

    public UnityEvent onPlayerDeath;
    public UnityEvent onPlayerMadness;

    public UnityEvent onReloadStart;
    public UnityEvent onReloadEnd;
    
    //Private Fileds
    private float reloadTimer = 0F;
    private bool reloading = false;
    
    private MainInputActionsSettings input;
    
    // Start is called before the first frame update
    void Start()
    {
        onCurrentHealthChange ??= new UnityEvent();
        onCurrentSanityChange ??= new UnityEvent();
        onCurrentAmmoChange ??= new UnityEvent();
        onCurrentStaminaChange ??= new UnityEvent();
        
        onPlayerDeath ??= new UnityEvent();
        onPlayerMadness ??= new UnityEvent();

        onReloadStart ??= new UnityEvent();
        onReloadEnd ??= new UnityEvent();

        reloadTimer = 0f;
        
        input = FindObjectOfType<GameStateBehaviourScript>().mainInputActions;
        
        input.Player.Reload.performed += ReloadOnPerformed;
    }

    private void OnDisable()
    {
        input.Player.Reload.performed -= ReloadOnPerformed;
    }

    // Update is called once per frame
    void Update()
    {
        float currentStaminaGain = chargeStaminaPerSec * Time.deltaTime;
        ChangeCurrentStamina(currentStaminaGain);

        if (reloading && reloadTimer <= 0)
        {
            currentAmmo = maxAmmo;
            reloading = false;
            onReloadEnd.Invoke();
            onCurrentAmmoChange.Invoke();
        }
        else
        {
            reloadTimer -= Time.deltaTime;
        }
    }

    public void ResetState()
    {
        currentAmmo = maxAmmo;
        currentHealth = maxHealth;
        currentSanity = maxSanity;
        currentStamina = maxStamina;
    }

    public bool ChangeCurrentHealth(float value)
    {
        var temp = currentHealth + value;
        if (value == 0) return true;
        if (!(0 <= temp && temp <= maxHealth)) return false;
        currentHealth = temp;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            onPlayerDeath.Invoke();
            return true;
        }
        onCurrentHealthChange.Invoke();
        return true;
    }

    public bool ChangeCurrentSanity(float value)
    {
        var temp = currentSanity + value;
        if (value == 0) return true;
        if (!(0 <= temp && temp <= maxSanity)) return false;
        currentSanity = temp;
        if (currentSanity == 0)
        {
            onPlayerMadness.Invoke();
        }
        onCurrentSanityChange.Invoke();
        return true;
    }
    
    public bool ChangeCurrentAmmo(float value)
    {
        if(reloading) return false;
        var temp = currentAmmo + value;
        if (value == 0) return true;
        if (!(0 <= temp && temp <= maxAmmo)) return false;
        currentAmmo = temp;
        onCurrentAmmoChange.Invoke();
        return true;
    }
    
    public bool ChangeCurrentStamina(float value)
    {
        var temp = currentStamina + value;
        if (value == 0) return true;
        if (!(0 <= temp && temp <= maxStamina)) return false;
        currentStamina = temp;
        onCurrentStaminaChange.Invoke();
        return true;
    }

    public void ReloadAmmo()
    {
        if (!reloading && (int) currentAmmo != (int) maxAmmo)
        {
            reloadTimer = reloadDelay;
            reloading = true;
            onReloadStart.Invoke();
        }
    }
    
    public float MAXHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public float MAXSanity
    {
        get => maxSanity;
        set => maxSanity = value;
    }

    public float MAXAmmo
    {
        get => maxAmmo;
        set => maxAmmo = value;
    }

    public float MAXStamina
    {
        get => maxStamina;
        set => maxStamina = value;
    }

    public float CurrentHealth => currentHealth;

    public float CurrentSanity => currentSanity;

    public float CurrentAmmo => currentAmmo;

    public float CurrentStamina => currentStamina;
    
    public float ReloadDelay => reloadDelay;
    
    private void ReloadOnPerformed(InputAction.CallbackContext obj)
    {
        ReloadAmmo();
    }
}
