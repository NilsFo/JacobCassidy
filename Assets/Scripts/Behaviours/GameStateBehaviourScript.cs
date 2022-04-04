using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameStateBehaviourScript : MonoBehaviour
{
    //Events 
    public Sprite cassDialogueSprite;
    public string[] cultistDeathQuotes; 
    
    public UnityEvent onResetGameState;
    public UnityEvent onCultistsDeath;

    public UnityEvent onWinGame;
    public UnityEvent onLoseGame;

    public UnityEvent onGamePause;
    public UnityEvent onGameResume;

    public UnityEvent onGameEnd;

    public MainInputActionsSettings mainInputActions;
    public TMP_Text questLogText;
    public bool isGameOver = false;

    //Refs
    [SerializeField] private PlayerStateBehaviourScript playerStateBehaviourScript;
    [SerializeField] private EnemieStateBehaviourScript enemieStateBehaviourScript;
    [SerializeField] private SpellStateBehaviourScript spellStateBehaviourScript;

    [SerializeField] private int numberOfCultists = 6;
    [SerializeField] private int numberOfDeadCultists = 0;

    public int NumberOfDeadCultists => numberOfDeadCultists;
    public int NumberOfCultists => numberOfCultists;


    // Start is called before the first frame update
    void Start()
    {
        ResetGameState();

        playerStateBehaviourScript.onPlayerDeath.AddListener(LoseGame);

        Play();
    }

    private void OnEnable()
    {
        onResetGameState ??= new UnityEvent();
        onCultistsDeath ??= new UnityEvent();

        onGamePause ??= new UnityEvent();
        onGameResume ??= new UnityEvent();

        onLoseGame ??= new UnityEvent();
        onWinGame ??= new UnityEvent();

        onGameEnd ??= new UnityEvent();

        mainInputActions = new MainInputActionsSettings();
    }

    private void OnDisable()
    {
        playerStateBehaviourScript.onPlayerDeath.RemoveListener(LoseGame);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateQuestLog();
    }

    public void UpdateQuestLog()
    {
        int c = NumberOfDeadCultists;
        if (c > 0)
        {
            questLogText.text = "Task: Interrogate Cultists: " + c + " / " + NumberOfCultists;
        }
        else
        {
            questLogText.text = "Task: Investigate the town.";
        }
    }

    private void ResetGameState()
    {
        playerStateBehaviourScript.ResetState();

        onResetGameState.Invoke();
    }

    public void RestartLevel()
    {
        onGameEnd.Invoke();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WinGame()
    {
        onWinGame.Invoke();
        onGameEnd.Invoke();
        Pause();
        isGameOver = true;
        ShowWinScreen();
    }

    public void LoseGame()
    {
        onLoseGame.Invoke();
        onGameEnd.Invoke();
        Pause();
        isGameOver = true;
        ShowLooseScreen();
    }

    public void Play()
    {
        mainInputActions.Toolbar.Enable();
        mainInputActions.Player.Enable();
        onGameResume.Invoke();
    }

    public void Pause()
    {
        mainInputActions.Toolbar.Disable();
        mainInputActions.Player.Disable();
        onGamePause.Invoke();
    }

    public PlayerStateBehaviourScript PlayerStateBehaviourScript => playerStateBehaviourScript;

    public EnemieStateBehaviourScript EnemieStateBehaviourScript => enemieStateBehaviourScript;

    public SpellStateBehaviourScript SpellStateBehaviourScript => spellStateBehaviourScript;

    public void AddCultistsDeath()
    {
        FindObjectOfType<ConversationUIBehaviourScript>().AddMsg(cassDialogueSprite, cultistDeathQuotes[numberOfDeadCultists]);
        numberOfDeadCultists++;
        onCultistsDeath.Invoke();

        if (numberOfDeadCultists >= numberOfCultists)
        {
            WinGame();
        }

    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void ShowWinScreen()
    {
        Debug.LogWarning("A WINNER IS YOU");
    }

    public void ShowLooseScreen()
    {
        Debug.LogWarning("LOST THE GAME");
    }
}
