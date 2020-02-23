using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainDestruction : MonoBehaviour, IDamageable
{

    public GameObject brain;
    public float brainHealth;
    public GameObject Particles;
    public void TakeDamage(float amount)

    {
        brainHealth -= amount;
        if (brainHealth <= 0)
        {
            Destroy(gameObject);
            Particles.SetActive(true);
        }
    }
    void Start()
    {
        Particles.SetActive(false);
    }

}
