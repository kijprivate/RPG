using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Player : MonoBehaviour,IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] int enemyLayer = 9;
    [SerializeField] float damage = 10f;
    [SerializeField] float minTimeBetweenHits = 0.5f;
    [SerializeField] float maxAttackRange = 2f;

    private CameraRaycaster camRay;
    GameObject currentTarget;
    float currentHealthPoints;
    float timeOfLastHit=0f;

    private void Start()
    {
        camRay = FindObjectOfType<CameraRaycaster>();
        camRay.notifyMouseClickObservers += OnMouseClicked;
        currentHealthPoints = maxHealthPoints;
    }

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }
    void OnMouseClicked(RaycastHit raycastHit, int layerHit)
    {
        if (layerHit == enemyLayer)
        {
            var enemy = raycastHit.collider.gameObject;

            if(Vector3.Distance(enemy.transform.position,transform.position) > maxAttackRange)
            { return; }

            currentTarget = enemy;
            var enemyComponent = currentTarget.GetComponent<Enemy>();
            if( Time.time - timeOfLastHit > minTimeBetweenHits)
            {
                enemyComponent.TakeDamage(damage);
                timeOfLastHit = Time.time;
            }
        }
    }
}
