using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileSpeed = 10f;

    float damageCaused = 10f;
    
    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }
    private void OnCollisionEnter(Collision other)
    {
        IDamageable damageableComponent = other.gameObject.GetComponent<IDamageable>();
        if (damageableComponent!=null)
        {
            damageableComponent.TakeDamage(damageCaused);
        }
        Destroy(gameObject,0.1f);
    }
}
