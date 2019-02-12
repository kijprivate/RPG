using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Weapons")]
public class Weapon : ScriptableObject {

    public Transform weaponGrid;

    [SerializeField] GameObject weaponPrefab;
    [SerializeField] AnimationClip attackAnimation;

    public GameObject GetWeaponPrefab()
    {
        return weaponPrefab;
    }
}
