using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

[Serializable]
public class PlayerPosSaveData
{
    public Vector3 playerPos;
    public Quaternion playerQuaternion;
    public Vector3 playerScale;
}

public class PlayerMovementBehaviour : MonoBehaviour, MainInputActions.IPlayerActions, ISaveable
{
    [Header("Dependency")] [SerializeField]
    private Rigidbody2D myRgidbody2D;

    public MovementAnimator movementAnimator;
    public PlayerInteractionManager playerInteractionManager;

    [Header("Settings")] [SerializeField] private float movementForce = 6000;
    public RuntimeInventory runtimeInventory;
    public RuntimePlayerInput runtimePlayerInput;
    public RuntimeGameState runtimeGameState;
    public RuntimeFollower runtimeFollower;

    public RuntimeSavePoint runtimeSavePoint;

    private MainInputActions _mainInputActions;

    private Vector2 _movementInput;

    private void Awake()
    {
        Debug.Assert(movementAnimator != null, gameObject);
        Debug.Assert(myRgidbody2D != null, gameObject);
        Debug.Assert(runtimeInventory != null, gameObject);
        Debug.Assert(runtimePlayerInput != null, gameObject);
        Debug.Assert(runtimeGameState != null, gameObject);
        Debug.Assert(runtimeFollower != null, gameObject);
        Debug.Assert(runtimeSavePoint != null, gameObject);
    }

    private void Update()
    {
        movementAnimator.velocity = _movementInput;
    }

    private void FixedUpdate()
    {
        myRgidbody2D.AddForce(_movementInput * movementForce * Time.fixedDeltaTime);
    }

    private void OnEnable()
    {
        _movementInput = Vector2.zero;
        _mainInputActions = runtimePlayerInput.GetMainInputActions();
        _mainInputActions.Player.SetCallbacks(this);

        runtimeSavePoint.AddSaveable(this);
    }

    private void OnDisable()
    {
        runtimeSavePoint.RemoveSaveable(this);
    }

    private void OnValidate()
    {
        if (movementAnimator == null) Debug.LogError("Player movement field 'movementAnimator' not set.", gameObject);

        if (movementAnimator == null) Debug.LogError("Player movement field 'myRgidbody2D' not set.", gameObject);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnContextAction(InputAction.CallbackContext context)
    {
        //Nothing done here
    }

    public void OnItemAction(InputAction.CallbackContext context)
    {
        //Nothing done here
    }

    public void OnFollowerAction(InputAction.CallbackContext context)
    {
        //Nothing done here
    }

    public void OnSpellCircleAction(InputAction.CallbackContext context)
    {
        //Nothing done here
    }

    public bool LoadData(ref Dictionary<string, string> data)
    {
        if (data != null && data.ContainsKey("PlayerPosSaveData") && !string.IsNullOrEmpty(data["PlayerPosSaveData"]))
        {
            var tempPlayerPosSaveData = JsonUtility.FromJson<PlayerPosSaveData>(data["PlayerPosSaveData"]);
            transform.position = tempPlayerPosSaveData.playerPos;
            transform.rotation = tempPlayerPosSaveData.playerQuaternion;
            transform.localScale = tempPlayerPosSaveData.playerScale;
        }

        return true;
    }

    public bool SaveData(ref Dictionary<string, string> data)
    {
        var tempPlayerPosSaveData = new PlayerPosSaveData();
        var myTransform = transform;
        tempPlayerPosSaveData.playerPos = myTransform.position;
        tempPlayerPosSaveData.playerQuaternion = myTransform.rotation;
        tempPlayerPosSaveData.playerScale = myTransform.localScale;

        if (string.IsNullOrEmpty(data[name]))
        {
            data["PlayerPosSaveData"] = JsonUtility.ToJson(tempPlayerPosSaveData);
            return true;
        }

        Debug.LogError("While Saving the Game, Duplicated DataEntry for PlayerPosSaveData!");

        return false;
    }

    public int GetUniqueId()
    {
        return gameObject.GetInstanceID();
    }
}