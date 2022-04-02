using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameStateBehaviourScript : MonoBehaviour
{
    
    //Events 
    public UnityEvent onResetGameState;

    //Refs
    [SerializeField] private PlayerStateBehaviourScript playerStateBehaviourScript;
    [SerializeField] private EnemieStateBehaviourScript enemieStateBehaviourScript;
    
    // Start is called before the first frame update
    void Start()
    {
        ResetGameState();
        
        playerStateBehaviourScript.onPlayerDeath.AddListener(RestartLevel);
    }

    private void OnEnable()
    {
        onResetGameState ??= new UnityEvent();
    }

    private void OnDisable()
    {
        playerStateBehaviourScript.onPlayerDeath.RemoveListener(RestartLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResetGameState()
    {
        playerStateBehaviourScript.ResetState();
        
        onResetGameState.Invoke();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
    
    public PlayerStateBehaviourScript PlayerStateBehaviourScript => playerStateBehaviourScript;
}
