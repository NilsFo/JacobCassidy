using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaminaBehaviourScript : MonoBehaviour
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
        UpdateStaminaText();
        _playerStateBehaviourScript.onCurrentStaminaChange.AddListener(UpdateStaminaText);
    }

    private void OnDisable()
    {
        _playerStateBehaviourScript.onCurrentStaminaChange.RemoveListener(UpdateStaminaText);
    }

    public void UpdateStaminaText()
    {
        goText.SetText(Mathf.Round(_playerStateBehaviourScript.CurrentStamina) + "/" + _playerStateBehaviourScript.MAXStamina);
    }
}
