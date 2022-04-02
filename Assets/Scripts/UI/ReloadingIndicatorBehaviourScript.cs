using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadingIndicatorBehaviourScript : MonoBehaviour
{
    
    [SerializeField] private PlayerStateBehaviourScript _playerStateBehaviourScript;

    [SerializeField] private GameObject goText;
    
    [SerializeField] private float effectDelay = 0.5f;
    [SerializeField] private float effectTime = 0;

    [SerializeField] private bool isAktive = false;
    
    private void Awake()
    {
        Debug.Assert(_playerStateBehaviourScript != null, gameObject);
        Debug.Assert(goText != null, gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        isAktive = false;
        effectTime = 0;
        
        goText.SetActive(false);
        _playerStateBehaviourScript.onReloadStart.AddListener(StartEffect);
        _playerStateBehaviourScript.onReloadEnd.AddListener(StopEffect);
    }

    private void OnDisable()
    {
        _playerStateBehaviourScript.onReloadStart.RemoveListener(StartEffect);
        _playerStateBehaviourScript.onReloadEnd.RemoveListener(StopEffect);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAktive)
        {
            if (effectTime <= 0)
            {
                effectTime = effectDelay;
                goText.SetActive(!goText.activeSelf);
            }
            effectTime -= Time.deltaTime;
        }
    }

    public void StartEffect()
    {
        goText.SetActive(!goText.activeSelf);
        effectTime = effectDelay;
        isAktive = true;
    }
    
    public void StopEffect()
    {
        goText.SetActive(false);
        effectTime = 0;
        isAktive = false;
    }
}
