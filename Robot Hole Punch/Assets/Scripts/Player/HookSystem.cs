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

    public float hookDistance = 20f;

    private bool isHooking = false;
    private Coroutine goToDestinationCoroutine = null;

    #endregion

    #region References

    private FirstPersonController FirstPersonController { get { return PlayerCenterControl.Instance.FirstPersonController; } }
    private CharacterController CharacterController { get { return FirstPersonController.CharacterController; } }
    private InputSystem input { get { return PlayerCenterControl.Instance.input; } }

    #endregion

    private void Update()
    {
        if (!isHooking)
        {
            if (input.Hook)
            {
                isHooking = true;
                Hook grapplingHook = Instantiate(hook, hookSpawn.position, hookSpawn.rotation).GetComponent<Hook>();
                grapplingHook.MaxDistance = hookDistance;
                grapplingHook.onHookShotComplete.AddListener(CheckHook);
            }
        }
    }

    public void CheckHook(Vector3 destination)
    {
        if (destination == Vector3.zero || Vector3.Distance(transform.position, destination) < 3f)
        {
            isHooking = false;
            return;
        }

        if (goToDestinationCoroutine != null)
            StopCoroutine(goToDestinationCoroutine);
        goToDestinationCoroutine = StartCoroutine(GoToDestination(destination));
    }

    private IEnumerator GoToDestination(Vector3 destination)
    {
        FirstPersonController.UseGravity = false;
        float curSpeed = hookStartSpeed;

        while (Vector3.Distance(transform.position, destination) > 1f)
        {
            Vector3 dir = (destination - transform.position).normalized;
            CharacterController.Move(dir * curSpeed * Time.deltaTime);
            curSpeed = Mathf.Lerp(curSpeed, hookEndSpeed, hookAcceleration * Time.deltaTime);
            yield return null;
        }

        FirstPersonController.UseGravity = true;
        isHooking = false;
    }


}