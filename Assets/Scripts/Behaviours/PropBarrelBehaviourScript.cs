using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBarrelBehaviourScript : MonoBehaviour
{

    public GameObject explotion;
    public SpriteRenderer renderer;
    public Collider2D polygonCollider2D;

    public void SpawnExplosion()
    {
        Instantiate(explotion, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject, 1f);
        polygonCollider2D.enabled = false;
        renderer.enabled = false;
    }
    
}
