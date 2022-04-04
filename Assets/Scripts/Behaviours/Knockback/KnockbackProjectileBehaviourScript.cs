using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackProjectileBehaviourScript : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 2000f;

    private bool knockback;
    private float lifetime = 1.5f;
    public ParticleSystem particles;

    void Start() {
        knockback = true;
    }
    
    void Update()
    {
        if (knockback) {
            knockback = false;
            List<Collider2D> colliders = new List<Collider2D>();
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.layerMask = LayerMask.GetMask(new []{"EnemyHitbox"});
            contactFilter2D.useLayerMask = true;
            var c = GetComponent<Collider2D>();
            c.OverlapCollider(contactFilter2D, colliders);
            
            // Displaying particles
            int count = Random.Range(25, 30);
            particles.Emit(count: count);

            Debug.Log("Knocking back " + colliders.Count + " enemies");
            
            var player = FindObjectOfType<PlayerMovementBehaviour>();
            foreach (var col in colliders) {
                var rb = col.GetComponentInParent<Rigidbody2D>();
                if (rb != null) {
                    Vector2 dir = (col.transform.position - player.transform.position);
                    rb.AddForce(dir.normalized * knockbackForce * (1-dir.magnitude/5f));
                }

                col.GetComponentInParent<ZombieAI>()?.SetStunTime(0.5f);
            }
        }

        lifetime -= Time.deltaTime;
        if (lifetime < 0) {
            Destroy(gameObject);
        }
    }
}
