using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthIndicatorBehaviourScript : MonoBehaviour
{
    
    [SerializeField] private PlayerStateBehaviourScript _playerStateBehaviourScript;
    
    [SerializeField] private TextMeshProUGUI goText;
    
    private void Awake()
    {
        Debug.Assert(_playerStateBehaviourScript != null, gameObject);
        Debug.Assert(goText != null, gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _playerStateBehaviourScript.onCurrentHealthChange.AddListener(UpdateHealthText);
    }

    private void OnDisable()
    {
        _playerStateBehaviourScript.onCurrentHealthChange.RemoveListener(UpdateHealthText);
    }

    public void UpdateHealthText()
    {
        goText.SetText(_playerStateBehaviourScript.CurrentHealth + "/" + _playerStateBehaviourScript.MAXHealth);
    }
}
