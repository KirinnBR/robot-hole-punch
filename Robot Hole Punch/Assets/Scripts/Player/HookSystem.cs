using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSystem : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Hook hook;
    [SerializeField]
    private float hookStartSpeed = 10f;
    [SerializeField]
    private float hookEndSpeed = 40f;
    [SerializeField]
    private float hookAcceleration = 10f;
    [SerializeField]
    private float hookDistance = 20f;

    private Vector3 hookshotPosition;
    private bool isHooking = false;
    private Coroutine tryDoHookCoroutine = null;

    private LayerMask environmentLayer { get { return LayerManager.Instance.environmentLayer; } }
    private LayerMask holeLayer { get { return LayerManager.Instance.holeLayer; } }

    #endregion

    #region References

    private Camera cam { get { return PlayerCenterControl.Instance.Camera; } }
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
                hook.Shoot(cam.transform.forward);
                //if (tryDoHookCoroutine != null)
                //    StopCoroutine(tryDoHookCoroutine);
                //tryDoHookCoroutine = StartCoroutine(TryDoHook());
            }
        }
    }

    private IEnumerator TryDoHook()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, hookDistance, holeLayer, QueryTriggerInteraction.Collide))
        {
            isHooking = Physics.Raycast(ray, out hit, hookDistance, environmentLayer, QueryTriggerInteraction.Ignore);
            if (isHooking)
            {
                FirstPersonController.UseGravity = false;
                float curSpeed = hookStartSpeed;
                hookshotPosition = hit.point;
                while (Vector3.Distance(transform.position, hookshotPosition) > 1f)
                {
                    Vector3 dir = (hookshotPosition - transform.position).normalized;
                    CharacterController.Move(dir * curSpeed * Time.deltaTime);
                    curSpeed = Mathf.Lerp(curSpeed, hookEndSpeed, hookAcceleration * Time.deltaTime);
                    yield return null;
                }
                isHooking = false;
                FirstPersonController.UseGravity = true;
            }
        }
        else
        {
            var hitDist = hit.distance;
            ray.origin = hit.point + cam.transform.forward;

            isHooking = Physics.Raycast(ray, out hit, hookDistance - hitDist, environmentLayer, QueryTriggerInteraction.Ignore);
            if (isHooking)
            {
                FirstPersonController.UseGravity = false;
                float curSpeed = hookStartSpeed;
                hookshotPosition = hit.point;
                while (Vector3.Distance(transform.position, hookshotPosition) > 1f)
                {
                    Vector3 dir = (hookshotPosition - transform.position).normalized;
                    CharacterController.Move(dir * curSpeed * Time.deltaTime);
                    curSpeed = Mathf.Lerp(curSpeed, hookEndSpeed, hookAcceleration * Time.deltaTime);
                    yield return null;
                }
                isHooking = false;
                FirstPersonController.UseGravity = true;
            }
        }
    }


}