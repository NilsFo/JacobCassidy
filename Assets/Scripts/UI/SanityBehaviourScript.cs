using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SanityBehaviourScript : MonoBehaviour
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
        _playerStateBehaviourScript.onCurrentSanityChange.AddListener(UpdateSanityText);
    }

    private void OnDisable()
    {
        _playerStateBehaviourScript.onCurrentSanityChange.RemoveListener(UpdateSanityText);
    }

    public void UpdateSanityText()
    {
        goText.SetText(_playerStateBehaviourScript.CurrentSanity + "/" + _playerStateBehaviourScript.MAXSanity);
    }
}
