using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SanityBehaviourScript : MonoBehaviour
{
    private PlayerStateBehaviourScript _playerStateBehaviourScript;
    
    [SerializeField] private Slider slider;
    
    private void Awake()
    {
        Debug.Assert(slider != null, gameObject);
    }
    
    void Start()
    {
        _playerStateBehaviourScript = FindObjectOfType<PlayerStateBehaviourScript>();
        
        _playerStateBehaviourScript.onCurrentSanityChange.AddListener(UpdateSanityText);
        
        UpdateSanityText();
    }

    private void OnDisable()
    {
        _playerStateBehaviourScript.onCurrentSanityChange.RemoveListener(UpdateSanityText);
    }

    public void UpdateSanityText()
    {
        slider.value = _playerStateBehaviourScript.CurrentSanity;
        slider.maxValue = _playerStateBehaviourScript.MAXSanity;
        slider.minValue = 0;
    }
}
