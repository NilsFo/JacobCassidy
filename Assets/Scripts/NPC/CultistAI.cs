using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CultistAI : MonoBehaviour
{
    public enum CultistState : UInt16
    {
        FindNextPointOfInterest,
        GoToPointOfInterest,
        SummonZombieSelf,
        SummonZombiePointOfInterest
    }

    public CultistPointOfInterestHolder pointOfInterestHolder;
    public GameObject zombiePrefab;
    private List<GameObject> myPointsOfInterest;
    [SerializeField] private int currentPointOfInterestIndex;
    public NPCMovementAI myMovement;

    [SerializeField] private bool canSeePlayer;
    public float playerDetectionDistance;

    [SerializeField] private bool playerSpotted;
    public CultistState currentState;
    private CultistState _lastState;
    private PlayerMovementBehaviour player;
    private PlayerStateBehaviourScript playerState;

    // My own zombies
    public int congregationSize = 3;
    public List<GameObject> congregation;

    private void Awake()
    {
        congregation = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        myPointsOfInterest = pointOfInterestHolder.pointsOfInterest;

        _lastState = currentState;
        myMovement.SetMovementStateStasis();
        player = FindObjectOfType<PlayerMovementBehaviour>();
        playerState = FindObjectOfType<PlayerStateBehaviourScript>();

        FindNextPointOfInterest();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();

        switch (currentState)
        {
            case CultistState.FindNextPointOfInterest:
                FindNextPointOfInterest();
                break;
            case CultistState.SummonZombieSelf:
                SummonZombie(true);
                break;
            case CultistState.SummonZombiePointOfInterest:
                SummonZombie(false);
                break;
            case CultistState.GoToPointOfInterest:
                if (NeedsZombie())
                {
                    currentState = CultistState.SummonZombieSelf;
                }
                else if (myMovement.ReachedPath && myMovement.GetDistanceToMovementTarget() < 2)
                {
                    if (GetCurrentPointOfInterest().NeedsZombie())
                    {
                        currentState = CultistState.SummonZombiePointOfInterest;
                    }
                    else
                    {
                        currentState = CultistState.FindNextPointOfInterest;
                    }
                }
                else
                {
                    // Walking....
                }

                break;
        }
    }

    public void UpdateState()
    {
        if (currentState == _lastState)
        {
            return;
        }

        print("The cultist state has changed from " + _lastState + " to " + currentState);
        _lastState = currentState;
        switch (currentState)
        {
            case CultistState.SummonZombieSelf:
                myMovement.SetMovementStateWaitHere();
                break;
            case CultistState.FindNextPointOfInterest:
                myMovement.SetMovementStateWaitHere();
                break;
            case CultistState.GoToPointOfInterest:
                myMovement.SetMovementStateMoveTo(myPointsOfInterest[currentPointOfInterestIndex]);
                break;
            case CultistState.SummonZombiePointOfInterest:
                myMovement.SetMovementStateWaitHere();
                break;
        }
    }

    private void SummonZombie(bool self)
    {
        print("New zombie summoned.");
        GameObject newZombie = Instantiate(zombiePrefab, transform.position, Quaternion.identity);

        if (self)
        {
            EnemyBehaviourScript script = newZombie.GetComponent<EnemyBehaviourScript>();
            congregation.Add(script.gameObject);
            script.OnDeath += OnCongregationZombieDeath;
        }
        else
        {
            GetCurrentPointOfInterest().AddZombie(newZombie);
        }

        currentState = CultistState.FindNextPointOfInterest;
    }

    private void OnCongregationZombieDeath(GameObject deadZombie)
    {
        congregation.Remove(deadZombie);
    }

    private void FindNextPointOfInterest()
    {
        List<int> POIsInNeed = new List<int>();
        for (int i = 0; i < myPointsOfInterest.Capacity; i++)
        {
            GameObject poi = myPointsOfInterest[i];
            CultistPointOfInterest interest = poi.GetComponent<CultistPointOfInterest>();
            if (interest.NeedsZombie())
            {
                POIsInNeed.Add(i);
            }
        }

        if (POIsInNeed.Count > 0)
        {
            int j = Random.Range(0, POIsInNeed.Capacity);
            currentPointOfInterestIndex = POIsInNeed[j];
        }
        else
        {
            currentPointOfInterestIndex = Random.Range(0, myPointsOfInterest.Capacity);
        }

        currentState = CultistState.GoToPointOfInterest;
    }

    private void FixedUpdate()
    {
        UpdateCanSeePlayer();
    }

    public void UpdateCanSeePlayer()
    {
        canSeePlayer = false;
        float dist = Vector2.Distance(transform.position, player.transform.position);

        if (dist < playerDetectionDistance)
        {
            canSeePlayer = true;
            var mask = LayerMask.GetMask("NavMeshBlocker");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position,
                distance: dist, layerMask: mask);
            if (hit.collider != null)
            {
                canSeePlayer = false;
            }
        }
    }

    public bool NeedsZombie()
    {
        return congregation.Count < congregationSize;
    }

    public CultistPointOfInterest GetCurrentPointOfInterest()
    {
        GameObject o = myPointsOfInterest[currentPointOfInterestIndex];
        return o.GetComponent<CultistPointOfInterest>();
    }
}