using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIndicatorBehaviourScript : MonoBehaviour
{

    public GameObject player;
    
    private List<CultistAI> list;
    
    [SerializeField] private float scanDelay = 2f;
    [SerializeField] private float scanTimer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        var result = FindObjectsOfType<CultistAI>();
        list = new List<CultistAI>(result);
    }

    // Update is called once per frame
    void Update()
    {
        if (scanTimer < 0)
        {
            FindClosed();
        }
        else
        {
            scanTimer -= Time.deltaTime;
        }
    }

    private void FindClosed()
    {
        var result = FindObjectsOfType<CultistAI>();
        list = new List<CultistAI>(result);

        CultistAI chosen = null;
        float lastdist = 0;
        
        foreach (var cultistAI in list)
        {
            float dist = Vector2.Distance(cultistAI.gameObject.transform.position, player.transform.position);
            if (dist < lastdist)
            {
                chosen = cultistAI;
            }
        }

        if (chosen != null)
        {
            var direction = chosen.gameObject.transform.position - player.transform.position;
            var rotaion = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f);

            transform.rotation = rotaion;
        }

        scanTimer = scanDelay;
    }
}
