using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : MonoBehaviour
{
    [SerializeField] private Transform debugHitPointtransform;

    public Camera playerCamera;

    private State state;

    public CharacterController characterController;

    private Vector3 hookshotPosition;

    private enum State
    {
        Normal,
        HookshotFlyingPlayer
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        state = State.Normal;
    }

    private void HandleHookshotStart()
    {
        if (Input.GetKeyDown(KeyCode.E))
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

                break;

        }
        HandleHookshotStart();
    }
    private void HandleHookshotMovement()
    {
        Vector3 hookshotDir = hookshotPosition - transform.position.normalized;

    float hookshotSpeed = 5f;

    characterController.Move(hookshotDir * hookshotSpeed * Time.deltaTime);
    }
}
