using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // The health script of the object hit by the weapon
    Health targetHealth;

    // The damage the weapon will do
    [SerializeField] float damage = 5;

    // Bool to enable and disable weapon
    [SerializeField] bool weaponEnabled = false;

    public void WeaponEnable()
    {
        weaponEnabled = true;
    }

    public void WeaponDisable()
    {
        weaponEnabled = false;
    }

    public void WeaponEnable(bool value)
    {
        weaponEnabled = value;
    }

    // When the weapon trigger collides with an object, do damage if it has a health script.
    private void OnTriggerEnter(Collider other)
    {
        if (weaponEnabled)
        {
            targetHealth = other.GetComponent<Health>();

            if (targetHealth != null)
            {
                targetHealth.ApplyDamage(damage, this.gameObject);
            }
        }
    }
}
