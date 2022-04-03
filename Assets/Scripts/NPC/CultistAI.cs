using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class CultistAI : MonoBehaviour
{
    public enum CultistState : UInt16
    {
        FindNextPointOfInterest,
        GoToPointOfInterest,
        SummonZombieSelf,
        SummonZombiePointOfInterest,
        Summoning,
        Dead,
        FleeFromPlayer,
        SummonDefense
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

    public float summonTime = 2f;
    private float _summonTimer;

    public int defenseSummons = 3;
    private int _defenseSummonsLeft;

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
                currentState = CultistState.Summoning;
                SummonZombie(true);
                break;
            case CultistState.SummonZombiePointOfInterest:
                currentState = CultistState.Summoning;
                SummonZombie(false);
                break;
            case CultistState.GoToPointOfInterest:
                if (canSeePlayer) {
                    FleeFromPlayer();
                    break;
                }
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
            case CultistState.Summoning:
                _summonTimer -= Time.deltaTime;
                if (_summonTimer < 0)
                {
                    currentState = CultistState.GoToPointOfInterest;
                    myMovement.myAnimator.myMovementAnimator.SetBool("Summoning", false);
                }

                break;
            case CultistState.FleeFromPlayer:
                if (myMovement.ReachedPath) {
                    currentState = CultistState.SummonDefense;
                    _defenseSummonsLeft = defenseSummons;
                    myMovement.myAnimator.myMovementAnimator.SetBool("Summoning", true);
                    myMovement.SetMovementStateWaitHere();
                    myMovement.movementSpeed /= 2;
                }
                break;
            case CultistState.SummonDefense:
                _summonTimer -= Time.deltaTime;
                if (_defenseSummonsLeft > 0 && _summonTimer < 0) {
                    _defenseSummonsLeft -= 1;
                    var zombie = SummonZombie(false);
                    zombie.currentState = ZombieAI.ZombieState.GoToLastKnownLocation;
                    _summonTimer = summonTime;
                }
                if (_defenseSummonsLeft == 0) {
                    currentState = CultistState.FindNextPointOfInterest;
                    myMovement.myAnimator.myMovementAnimator.SetBool("Summoning", false);
                }
                break;
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            //Debug.DrawLine(sourceNPC.transform.position, roamingOrigin);
            Vector3 wireOrigin = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
            Handles.Label(transform.position, "State: " + currentState);
        }
#endif
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
            case CultistState.Summoning:
                myMovement.SetMovementStateWaitHere();
                break;
        }
    }
    
    private ZombieAI SummonZombie(bool self)
    {
        print("New zombie summoned.");
        GameObject newZombie = Instantiate(zombiePrefab, new Vector3(transform.position.x, transform.position.y, 0),
            Quaternion.identity);
        newZombie.GetComponent<MovementAnimator>().myMovementAnimator.SetTrigger("Spawn");

        ZombieAI zombieAI = newZombie.GetComponent<ZombieAI>();
        zombieAI.myCultist = gameObject;
        zombieAI.SetStunTime(2f);
        zombieAI.hitbox.enabled = false;
        zombieAI.EnableHitboxIn2s();

        if (self)
        {
            EnemyBehaviourScript script = newZombie.GetComponent<EnemyBehaviourScript>();
            congregation.Add(script.gameObject);
            script.OnDeath += OnCongregationZombieDeath;
            zombieAI.currentState = ZombieAI.ZombieState.FollowCultist;
        }
        else
        {
            GetCurrentPointOfInterest().AddZombie(newZombie);
            zombieAI.currentState = ZombieAI.ZombieState.Roaming;
        }


        _summonTimer = summonTime;
        myMovement.myAnimator.myMovementAnimator.SetBool("Summoning", true);
        return zombieAI;
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

    public void Die()
    {
        myMovement.myAnimator.myMovementAnimator.SetTrigger("Die");
        myMovement.SetMovementStateStasis();
        currentState = CultistState.Dead;
        var debrisLayer = LayerMask.NameToLayer("Debris");
        gameObject.layer = debrisLayer;
        foreach (var componentsInChild in gameObject.GetComponentsInChildren<Collider2D>())
        {
            componentsInChild.gameObject.layer = debrisLayer;
        }

        // Announce to the world that I have died
        FindObjectOfType<GameStateBehaviourScript>().AddCultistsDeath();
        
        ZombieEncounter[] encounters = FindObjectsOfType<ZombieEncounter>();
        foreach (ZombieEncounter encounter in encounters)
        {
            encounter.TrySpawn();
        }

        // Announce to the points of interest that more zombies are needed
        CultistPointOfInterest[] holders = FindObjectsOfType<CultistPointOfInterest>();
        foreach (CultistPointOfInterest holder in holders)
        {
            holder.congregationSize = holder.congregationSize + 2;
        }
    }
    
    private void FleeFromPlayer() {
        currentState = CultistState.FleeFromPlayer;


        float maxDist = 0;
        GameObject gotopoi = null;
        // Find point of interest furthest away from player
        foreach (var poi in myPointsOfInterest) {
            var dist = (poi.transform.position - player.transform.position).magnitude;
            if (dist > maxDist) {
                gotopoi = poi;
                maxDist = dist;
            }
        }
        myMovement.movementSpeed *= 2;
        myMovement.SetMovementStateMoveTo(gotopoi);
    }
}
