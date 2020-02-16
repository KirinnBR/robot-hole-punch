using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNPC : MonoBehaviour, IDamageable
{
    public Stats stats;
    public float CurrentHealth { get; private set; }
    private void Start()
    {
        CurrentHealth = stats.health;
    }
    public void TakeDamage(float amount)
    {
        Debug.Log(name + " took " + amount + " damage.");
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
