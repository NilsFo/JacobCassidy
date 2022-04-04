using UnityEngine;

public class ReloadingIndicatorBehaviourScript : MonoBehaviour
{
    
    [SerializeField] private PlayerStateBehaviourScript playerStateBehaviourScript;

    [SerializeField] private float effectMulti = 2f;
    
    private float effectDuration = 0.5f;
    private float effectTime = 0;

    public RectTransform transform;
    
    // Start is called before the first frame update
    void Start()
    {
        
        effectTime = 0;

        playerStateBehaviourScript = FindObjectOfType<PlayerStateBehaviourScript>();

        effectDuration = playerStateBehaviourScript.ReloadDelay / effectMulti;
        
        playerStateBehaviourScript.onReloadStart.AddListener(StartEffect);
        playerStateBehaviourScript.onReloadEnd.AddListener(StopEffect);
    }

    private void OnDisable()
    {
        playerStateBehaviourScript.onReloadStart.RemoveListener(StartEffect);
        playerStateBehaviourScript.onReloadEnd.RemoveListener(StopEffect);
    }

    // Update is called once per frame
    void Update()
    {
        if (effectTime != 0)
        {
            effectTime -= Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, 360 * (effectTime/effectDuration));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void StartEffect()
    {
        effectTime = effectDuration;
    }
    
    public void StopEffect()
    {
        effectTime = 0;
    }
}
