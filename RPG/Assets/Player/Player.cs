using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.Assertions;

public class Player : MonoBehaviour,IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] int enemyLayer = 9;
    [SerializeField] float damage = 10f;
    [SerializeField] float minTimeBetweenHits = 0.5f;
    [SerializeField] float maxAttackRange = 2f;

    [SerializeField] Weapon weapon;

    private CameraRaycaster camRay;
    GameObject currentTarget;
    float currentHealthPoints;
    float timeOfLastHit=0f;
    Animator animator;

    private void Start()
    {
        camRay = FindObjectOfType<CameraRaycaster>();
        camRay.notifyMouseClickObservers += OnMouseClicked;
        currentHealthPoints = maxHealthPoints;
        animator = GetComponent<Animator>();

        PutWeaponInHand();
    }

    private void PutWeaponInHand()
    {
        var weaponPrefab = weapon.GetWeaponPrefab();
        var dominantHand = RequestDominantHand();
        Instantiate(weaponPrefab, dominantHand.transform);
        weaponPrefab.transform.localPosition = weapon.weaponGrid.localPosition;
        weaponPrefab.transform.localRotation = weapon.weaponGrid.localRotation;
    }

    private GameObject RequestDominantHand()
    {
        var dominantHands = GetComponentsInChildren<DominantHand>();
        int numberOfDominantHands = dominantHands.Length;
        Assert.AreNotEqual(numberOfDominantHands, 0, "Couldnt find dominant hand");
        Assert.IsFalse(numberOfDominantHands > 1, "Multiple dominant hands");
        return dominantHands[0].gameObject;
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
                animator.SetTrigger("Attack");
                timeOfLastHit = Time.time;
            }
        }
    }
}
