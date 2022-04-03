using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameStateBehaviourScript : MonoBehaviour
{
    
    //Events 
    public UnityEvent onResetGameState;
    public UnityEvent onCultistsDeath;
    
    public MainInputActionsSettings mainInputActions;

    //Refs
    [SerializeField] private PlayerStateBehaviourScript playerStateBehaviourScript;
    [SerializeField] private EnemieStateBehaviourScript enemieStateBehaviourScript;
    [SerializeField] private SpellStateBehaviourScript spellStateBehaviourScript;

    [SerializeField] private int numberOfDeadCultists = 0;

    public int NumberOfDeadCultists => numberOfDeadCultists;

    // Start is called before the first frame update
    void Start()
    {
        ResetGameState();
        
        playerStateBehaviourScript.onPlayerDeath.AddListener(RestartLevel);
    }

    private void OnEnable()
    {
        onResetGameState ??= new UnityEvent();
        onCultistsDeath ??= new UnityEvent();
        mainInputActions = new MainInputActionsSettings();
        mainInputActions.Player.Enable();
        mainInputActions.Toolbar.Enable();
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

    public EnemieStateBehaviourScript EnemieStateBehaviourScript => enemieStateBehaviourScript;

    public SpellStateBehaviourScript SpellStateBehaviourScript => spellStateBehaviourScript;

    public void AddCultistsDeath()
    {
        numberOfDeadCultists++;
        onCultistsDeath.Invoke();
    }
    
}
