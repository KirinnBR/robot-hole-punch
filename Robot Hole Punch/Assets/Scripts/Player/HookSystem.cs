using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSystem : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject hook;
    [SerializeField]
    private Transform hookSpawn;
    [SerializeField]
    private float hookStartSpeed = 10f;
    [SerializeField]
    private float hookEndSpeed = 40f;
    [SerializeField]
    private float hookAcceleration = 10f;
    [SerializeField]
    private AudioClip hookClip;

    public float hookDistance = 20f;

    private bool isHooking = false;
    private Coroutine goToDestinationCoroutine = null;
    public bool CanHook { get; set; } = true;

    #endregion

    #region References

    private FirstPersonController firstPersonController { get { return PlayerCenterControl.Instance.FirstPersonController; } }
    private CharacterController characterController { get { return firstPersonController.CharacterController; } }
    private CombatSystem combat { get { return PlayerCenterControl.Instance.Combat; } }
    private InputSystem input { get { return PlayerCenterControl.Instance.input; } }
    private AudioSource audio { get { return PlayerCenterControl.Instance.Audio; } }

    #endregion

    private void Update()
    {
        if (!isHooking)
        {
            if (input.Hook)
            {
                if (!CanHook) return;

                firstPersonController.UseSound = false;
                combat.CanCharge = false;

                audio.clip = hookClip;
                audio.Play();

                isHooking = true;
                Hook grapplingHook = Instantiate(hook, hookSpawn.position, Quaternion.LookRotation(-hookSpawn.forward, hookSpawn.up)).GetComponent<Hook>();
                grapplingHook.hookCaller = transform;
                grapplingHook.MaxDistance = hookDistance;
                grapplingHook.onHookShotComplete.AddListener(CheckHook);
            }
        }
    }

    public void CheckHook(Vector3 destination)
    {
        if (destination == Vector3.zero || Vector3.Distance(transform.position, destination) < 3f)
        {
            firstPersonController.UseSound = true;
            combat.CanCharge = true;
            isHooking = false;
            return;
        }

        if (goToDestinationCoroutine != null)
            StopCoroutine(goToDestinationCoroutine);
        goToDestinationCoroutine = StartCoroutine(GoToDestination(destination));
    }

    private IEnumerator GoToDestination(Vector3 destination)
    {
        firstPersonController.UseGravity = false;
        float curSpeed = hookStartSpeed;

        while (Vector3.Distance(transform.position, destination) > 1f)
        {
            Vector3 dir = (destination - transform.position).normalized;
            characterController.Move(dir * curSpeed * Time.deltaTime);
            curSpeed = Mathf.Lerp(curSpeed, hookEndSpeed, hookAcceleration * Time.deltaTime);
            yield return null;
        }

        firstPersonController.UseGravity = true;
        firstPersonController.UseSound = true;
        combat.CanCharge = true;

        isHooking = false;
    }

}