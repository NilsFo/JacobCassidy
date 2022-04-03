using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

public class DoorBehaviourScript : MonoBehaviour
{

    public SpriteRenderer renderer;
    public BoxCollider2D boxCollider2D;
    
    public List<Sprite> list;
    
    public UnityEvent onDeath;
    public UnityEvent onDamageTaken;
    
    public int maxHealth = 4;
    public int currentHealth = 4;
    
    private AstarPath path;
    private Bounds bounds;
    
    void Start()
    {
        list ??= new List<Sprite>();

        onDeath ??= new UnityEvent();
        onDamageTaken ??= new UnityEvent();
        
        path = FindObjectOfType<AstarPath>();
        bounds = GetComponent<Collider2D>().bounds;
        
        currentHealth = maxHealth;
    }

    private void SetState()
    {
        renderer.sprite = list[currentHealth];
        if (currentHealth == 0)
        {
            boxCollider2D.enabled = false;
            UpdateNavmesh();
        }
    }

    public bool DamageHealth(int value)
    {
        currentHealth -= value;
        SetState();
        if (currentHealth <= 0)
        {
            onDeath.Invoke();
        }
        onDamageTaken.Invoke();
        return true;
    }
    
    void UpdateNavmesh() {
        // Updating pathfinding
        GraphUpdateObject guo = new GraphUpdateObject(bounds);
        guo.updatePhysics = true;
        path.UpdateGraphs(guo);
    }
}
