using UnityEngine;
using UnityEngine.UI;

public class HealthIndicatorBehaviourScript : MonoBehaviour
{
    
    PlayerStateBehaviourScript _playerStateBehaviourScript;
    
    [SerializeField] private Slider slider;
    
    private void Awake()
    {
        Debug.Assert(slider != null, gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _playerStateBehaviourScript = FindObjectOfType<PlayerStateBehaviourScript>();
        
        _playerStateBehaviourScript.onCurrentHealthChange.AddListener(UpdateHealthText);

        UpdateHealthText();
    }

    private void OnDisable()
    {
        _playerStateBehaviourScript.onCurrentHealthChange.RemoveListener(UpdateHealthText);
    }

    public void UpdateHealthText()
    {
        slider.value = _playerStateBehaviourScript.CurrentHealth;
        slider.maxValue = _playerStateBehaviourScript.MAXHealth;
        slider.minValue = 0;
    }
}
