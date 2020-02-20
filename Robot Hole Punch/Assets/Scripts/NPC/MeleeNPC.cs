using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeNPC : NPC
{
    #region Data Types

    [System.Serializable]
    public enum PatrollingType
    {
        Loop, Rewind
    }

    [System.Serializable]
    public enum GuardType
    {
        KeepPosition, FollowPlayerLastTrack
    }

    [System.Serializable]
    public struct PatrollerSettings
    {
        public PatrollingType patrollingType;
        public float patrollingWaitTime;
        public Transform[] patrolPoints;
    }

    private PatrollingType patrollingType { get { return patrollerSettings.patrollingType; } }
    private float patrollingWaitTime { get { return patrollerSettings.patrollingWaitTime; } }
    private Transform[] patrolPoints { get { return patrollerSettings.patrolPoints; } }

    [System.Serializable]
    public struct GuardSettings
    {
        public GuardType guardType;
        [Range(1, 359)]
        public float guardAngleTrack;
    }

    private GuardType guardType { get { return guardSettings.guardType; } }
    private float angleTrack { get { return guardSettings.guardAngleTrack; } }

    #endregion

    [Header("Hit Sphere Settings")]

    [SerializeField]
    private Vector3 hitSphereOffset = Vector3.zero;
    [SerializeField]
    private float hitSphereRadius = 1f;

    [Header("Combat Settings")]

    [SerializeField]
    private float timeBetweenAttacks = 1.2f;

    [Header("Behaviour Settings")]

    [SerializeField]
    private bool isPatroler = false;
    [SerializeField]
    [ConditionalHide("isPatroler", true)]
    private PatrollerSettings patrollerSettings;
    [ConditionalHide("isPatroler", false)]
    [SerializeField]
    private GuardSettings guardSettings;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    

    private Coroutine patrolModeCoroutine = null, combatModeCoroutine = null, guardModeCoroutine = null;

    private bool canAttack = true;
    private bool onWaitingMode = false;
    private bool onCombatMode = false;
    private int currentPatrolPoint = 0;

    protected override void Start()
    {
        base.Start();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (seePlayer)
        {
            if (!onCombatMode)
            {
                StopAllCoroutines();
                if (combatModeCoroutine != null)
                    StopCoroutine(combatModeCoroutine);
                combatModeCoroutine = StartCoroutine(CombatMode());
            }
        }
        else
        {
            if (!onWaitingMode)
            {
                StopAllCoroutines();
                if (isPatroler)
                {
                    if (patrolModeCoroutine != null)
                        StopCoroutine(patrolModeCoroutine);
                    patrolModeCoroutine = StartCoroutine(PatrolMode());
                }
                else
                {
                    if (guardModeCoroutine != null)
                        StopCoroutine(guardModeCoroutine);
                    guardModeCoroutine = StartCoroutine(GuardMode());
                }
            }
            
        }
    }

    private IEnumerator PatrolMode()
    {
        onWaitingMode = true;
        onCombatMode = false;

        yield return new WaitUntil(() => IsCloseEnoughToPoint(agent.destination));
        yield return new WaitForSeconds(patrollingWaitTime);

        if (patrolPoints.Length == 0) yield break;

        if (patrollingType == PatrollingType.Loop)
        {
            while (true)
            {
                agent.SetDestination(patrolPoints[currentPatrolPoint].position);
                yield return new WaitUntil(() => IsCloseEnoughToPoint(agent.destination));
                yield return new WaitForSeconds(patrollingWaitTime);
                currentPatrolPoint = currentPatrolPoint == patrolPoints.Length - 1 ? 0 : currentPatrolPoint + 1;
            }
        }
        else if (patrollingType == PatrollingType.Rewind)
        {
            bool rewinding = false;
            while (true)
            {
                agent.SetDestination(patrolPoints[currentPatrolPoint].position);

                yield return new WaitUntil(() => IsCloseEnoughToPoint(agent.destination));
                yield return new WaitForSeconds(patrollingWaitTime);

                if (!rewinding)
                {
                    if (currentPatrolPoint == patrolPoints.Length - 1)
                    {
                        rewinding = true;
                        currentPatrolPoint--;
                    }
                    else
                    {
                        currentPatrolPoint++;
                    }
                }
                else
                {
                    if (currentPatrolPoint == 0)
                    {
                        rewinding = false;
                        currentPatrolPoint++;
                    }
                    else
                    {
                        currentPatrolPoint--;
                    }
                }
            }
        }

    }

    private IEnumerator GuardMode()
    {
        onWaitingMode = true;
        onCombatMode = false;

        if (guardType == GuardType.FollowPlayerLastTrack)
        {
            yield return new WaitUntil(() => IsCloseEnoughToPoint(agent.destination));
        }
        else if (guardType == GuardType.KeepPosition)
        {
            agent.destination = initialPosition;
            yield return new WaitUntil(() => IsCloseEnoughToPoint(agent.destination));
            transform.rotation = initialRotation;
        }

        bool isRotationForward = true;

        Quaternion startAngle = reference.rotation;
        Quaternion desiredAngleFwd = Quaternion.Euler(startAngle.eulerAngles.x, startAngle.eulerAngles.y + (angleTrack/2), startAngle.eulerAngles.z);
        Quaternion desiredAngleBkd = Quaternion.Euler(startAngle.eulerAngles.x, startAngle.eulerAngles.y - (angleTrack / 2), startAngle.eulerAngles.z);

        while (true)
        {
            if (isRotationForward)
            {
                reference.rotation = Quaternion.Slerp(reference.rotation, desiredAngleFwd , 3f * Time.deltaTime);
                if (Quaternion.Angle(reference.rotation, desiredAngleFwd) <= 1f)
                {
                    reference.rotation = desiredAngleFwd;
                    isRotationForward = false;
                }
            }
            else
            {
                reference.rotation = Quaternion.Slerp(reference.rotation, desiredAngleBkd, 3f * Time.deltaTime);
                if (Quaternion.Angle(reference.rotation, desiredAngleBkd) <= 1f)
                {
                    reference.rotation = desiredAngleBkd;
                    isRotationForward = true;
                }
            }
            yield return null;
        }

    }

    private IEnumerator CombatMode()
    {
        onCombatMode = true;
        onWaitingMode = false;
        reference.eulerAngles = new Vector3(reference.eulerAngles.x, 0f, transform.eulerAngles.z);
        while (playerTransform != null)
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
