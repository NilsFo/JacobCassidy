using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoIndicatorBehaviourScript : MonoBehaviour
{
    
    private PlayerStateBehaviourScript _playerStateBehaviourScript;

    [SerializeField] private List<Sprite> ammoImgsList;

    [SerializeField] private Image img;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerStateBehaviourScript = FindObjectOfType<PlayerStateBehaviourScript>();
        
        ammoImgsList ??= new List<Sprite>();
        
        _playerStateBehaviourScript.onCurrentAmmoChange.AddListener(UpdateAmmoIndicator);
    }

    private void OnDisable()
    {
        _playerStateBehaviourScript.onCurrentAmmoChange.RemoveListener(UpdateAmmoIndicator);
    }

    public void UpdateAmmoIndicator()
    {
        var numberOfAmmo = _playerStateBehaviourScript.CurrentAmmo;
        img.sprite = ammoImgsList[(int)numberOfAmmo];
    }
}
