using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoIndicatorBehaviourScript : MonoBehaviour
{
    
    [SerializeField] private PlayerStateBehaviourScript _playerStateBehaviourScript;

    [SerializeField] private List<GameObject> ammoImgs;
    
    private void Awake()
    {
        Debug.Assert(_playerStateBehaviourScript != null, gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ammoImgs ??= new List<GameObject>();
        
        _playerStateBehaviourScript.onCurrentAmmoChange.AddListener(UpdateAmmoIndicator);
    }

    private void OnDisable()
    {
        _playerStateBehaviourScript.onCurrentAmmoChange.RemoveListener(UpdateAmmoIndicator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateAmmoIndicator()
    {
        Debug.Log("Render Ammo!");
        var numberOfAmmo = _playerStateBehaviourScript.CurrentAmmo;
        for (int i = 0; i < ammoImgs.Count; i++)
        {
            var imgOb = ammoImgs[i];
            if (i < numberOfAmmo)
            {
                imgOb.SetActive(true);
            }
            else
            {
                imgOb.SetActive(false);
            }
        }
    }
}
