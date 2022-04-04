using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SanityBehaviourScript : MonoBehaviour
{
    private PlayerStateBehaviourScript _playerStateBehaviourScript;
    
    [SerializeField] private Slider slider;
    public RectTransform rect;

    private float timer = 0f;
    private float duration = 0.5f;
    
    private bool isShacken = false;
    
    private void Awake()
    {
        Debug.Assert(slider != null, gameObject);
    }
    
    void Start()
    {
        _playerStateBehaviourScript = FindObjectOfType<PlayerStateBehaviourScript>();
        
        _playerStateBehaviourScript.onCurrentSanityChange.AddListener(UpdateSanityText);
        _playerStateBehaviourScript.onPlayerMadness.AddListener(Shack);

        UpdateSanityText();
    }

    private void Update()
    {
        if (isShacken)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                slider.gameObject.SetActive(!slider.gameObject.activeSelf);
                timer = duration;
            }
        }
        else
        {
            slider.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        _playerStateBehaviourScript.onCurrentSanityChange.RemoveListener(UpdateSanityText);
        _playerStateBehaviourScript.onPlayerMadness.RemoveListener(Shack);
    }

    public void UpdateSanityText()
    {
        slider.value = _playerStateBehaviourScript.CurrentSanity;
        slider.maxValue = _playerStateBehaviourScript.MAXSanity;
        slider.minValue = 0;
        if (_playerStateBehaviourScript.CurrentSanity != 0)
        {
            isShacken = false;
        }
    }

    public void Shack()
    {
        isShacken = true;
        timer = duration;
        slider.gameObject.SetActive(false);
    }
}
