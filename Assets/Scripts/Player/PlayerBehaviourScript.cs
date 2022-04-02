using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour
{
    
    [SerializeField] private PlayerStateBehaviourScript _playerStateBehaviourScript;

    public PlayerStateBehaviourScript PlayerStateBehaviourScript => _playerStateBehaviourScript;
}
