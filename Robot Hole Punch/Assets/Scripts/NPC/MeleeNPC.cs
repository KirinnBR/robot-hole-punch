using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeNPC : NPC
{

    [SerializeField]
    private Vector3 hitSphereOffset = Vector3.zero;
    [SerializeField]
    private float hitSphereRadius = 1f;
    [SerializeField]
    private float timeBetweenAttacks = 1.2f;

    private bool canAttack = true;

    private bool onCombatMode = false;

    private void Update()
    {
        if (seePlayer)
        {
            if (!onCombatMode)
            {
                StartCoroutine(CombatMode());
            }
        }
    }


    private IEnumerator CombatMode()
    {
        onCombatMode = true;
        while (true)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) > wideDistanceVisionRadius)
            {
                break;
            }

            if (canAttack)
            {
                if (IsCloseEnoughToPoint(playerTransform.position))
                {
                    playerDMG.TakeDamage(stats.damage);
                    StartCoroutine(ResetAttack());
                }
                agent.SetDestination(playerTransform.position);
            }
            else
            {
                var dist = Vector3.Distance(transform.position, playerTransform.position);
                if (dist < 5f)
                    agent.SetDestination(transform.position - (transform.forward * 2f));
                else if (dist > 10f)
                    agent.SetDestination(playerTransform.position);
                
            }
            transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
            yield return null;
        }
        onCombatMode = false;
    }

    private IEnumerator ResetAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(timeBetweenAttacks);
        canAttack = true;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + hitSphereOffset, hitSphereRadius);
    }


}
