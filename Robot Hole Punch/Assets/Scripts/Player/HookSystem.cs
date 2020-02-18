using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSystem : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Transform debugHitPointtransform;

    private State state;
    private Vector3 hookshotPosition;

    [SerializeField]
    private FirstPersonController firstPersonController;
    private Vector3 characterVelocityMomentum;

    #endregion

    #region References

    private Camera playerCamera { get { return PlayerCenterControl.Instance.Camera; } }
    private CharacterController characterController { get { return PlayerCenterControl.Instance.CharacterController; } }
    private InputSystem input { get { return PlayerCenterControl.Instance.input; } }

    #endregion

    private enum State
    {
        Normal,
        HookshotFlyingPlayer
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        state = State.Normal;
    }

    private void HandleHookshotStart()
    {
        if (input.Hook)
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit))
            {
                debugHitPointtransform.position = raycastHit.point;
                hookshotPosition = raycastHit.point;
                state = State.HookshotFlyingPlayer;
            }
        }
    }

    private void HandleCharacterLook()
    {
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");
        //// no idea if this works
    }

    private void HandleCharacterMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.Normal:
                HandleCharacterLook();
                HandleCharacterMovement();
                HandleHookshotStart();
                break;
            case State.HookshotFlyingPlayer:
                HandleHookshotMovement();
                HandleCharacterLook();
                break;

        }
        HandleHookshotStart();
    }
    private void HandleHookshotMovement()

    {
        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        float hookshotSpeedMin = 10f;
        float hookshotSpeedMax = 40f;

        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 2f;

        firstPersonController.useGravity = false;

        characterController.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);

        float reachedHookshotPositionDistance = 1f;

        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)
        {
            state = State.Normal;
            firstPersonController.useGravity = true;
        }

        if (TestInputDownHookshot())
        {
            state = State.Normal;
            firstPersonController.useGravity = true;

        }

        if (TestInputJump())
        {

        }

    }

    private bool TestInputDownHookshot()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    private bool TestInputJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
