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

    #endregion

    #region References

    private Camera playerCamera { get { return PlayerCenterControl.Instance.Camera; } }
    private CharacterController characterController { get { return PlayerCenterControl.Instance.CharacterController; } }
    private FirstPersonController firstPersonController { get { return PlayerCenterControl.Instance.FirstPersonController; } }
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
        firstPersonController.CanUseGravity = true;
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
                break;

        }
        HandleHookshotStart();
    }
    private void HandleHookshotMovement()
    {
        firstPersonController.CanUseGravity = false;

        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        float hookshotSpeed = 5f;

        characterController.Move(hookshotDir * hookshotSpeed * Time.deltaTime);
    }

}
