using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestIndicatorBehaviourScript : MonoBehaviour
{

    public SpriteRenderer renderer;
    public GameObject player;
    
    private List<CultistAI> list;
    private CultistAI chosen = null;
    
    [SerializeField] private float scanDelay = 2f;
    [SerializeField] private float scanTimer = 0f;

    [SerializeField] private float aktiveDuration = 0.5f;
    [SerializeField] private float aktiveTimer = 0f;
    
    private bool isAktie = false;
    
    private MainInputActionsSettings input;
    
    // Start is called before the first frame update
    void Start()
    {
        scanTimer = 0;
        aktiveTimer = 0;
        
        input = FindObjectOfType<GameStateBehaviourScript>().mainInputActions;
        
        input.Player.Quest.performed += QuestOnPerformed;
    }

    private void QuestOnPerformed(InputAction.CallbackContext obj)
    {
        if(!isAktie)
        {
            isAktie = true;
            aktiveTimer = aktiveDuration;
            FindClosed();
        }
    }

    private void OnDisable()
    {
        input.Player.Quest.performed -= QuestOnPerformed;
    }

    // Update is called once per frame
    void Update() {
        if (aktiveTimer >= 0) {
            aktiveTimer -= Time.deltaTime;
            if (aktiveTimer < 0) {
                renderer.enabled = false;
                isAktie = false;
            }
        }
    }

    private void FindClosed()
    {
        var result = FindObjectsOfType<CultistAI>();
        list = new List<CultistAI>(result);

        float lastdist = 999999999;
        foreach (var cultistAI in list)
        {
            float dist = Vector2.Distance(cultistAI.gameObject.transform.position, player.transform.position);
            if (dist < lastdist && cultistAI.currentState != CultistAI.CultistState.Dead)
            {
                chosen = cultistAI;
                lastdist = dist;
            }
        }
        
        if (chosen != null)
        {
            var direction = chosen.gameObject.transform.position - player.transform.position;
            var rotaion = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);

            transform.rotation = rotaion;
            renderer.enabled = true;
        }
        else
        {
            renderer.enabled = false;
        }
    }
}
