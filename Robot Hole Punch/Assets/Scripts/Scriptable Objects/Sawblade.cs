using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawblade : MonoBehaviour
{

    private CombatSystem CombatSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (tag == ("Player"))
        {


            CombatSystem.TakeDamage(100);
        }
    }
}
