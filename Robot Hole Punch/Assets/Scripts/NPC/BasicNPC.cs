using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicNPC : NPC
{
    private Transform enemy;
    private IDamageable dmg;

    private void Update()
    {
        if (hasVisibleObjects)
        {
            enemy = visibleObjects.Find(en => en.CompareTag("Player"));
            dmg = enemy.GetComponent<IDamageable>();
        }

        if (enemy != null)
        {
            agent.destination = enemy.position;
            if (IsCloseEnoughToPoint(enemy.position))
            {
                dmg.TakeDamage(2f);
            }
        }


    }
}
