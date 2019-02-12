using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject shooter;

    float damageCaused = 10f;
    
    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }
    public float GetDefaultProjectileSpeed()
    {
        return projectileSpeed;
    }
    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }
    private void OnCollisionEnter(Collision other)
    {
        IDamageable damageableComponent = other.gameObject.GetComponent<IDamageable>();
        if (damageableComponent!=null && shooter.layer != other.gameObject.layer)
        {
            damageableComponent.TakeDamage(damageCaused);
        }
        Destroy(gameObject,0.1f);
    }
}
