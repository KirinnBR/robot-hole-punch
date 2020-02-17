using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(CombatSystem), typeof(InputSystem), typeof(FirstPersonController))]
public class PlayerCenterControl : Singleton<PlayerCenterControl>
{
    public InputSystem input { get; private set; }
    [SerializeField]
    private Camera cam;
    public Camera Camera { get { return cam; } }
    public CharacterController CharacterController { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        if (cam == null)
            cam = Camera.main;
        CharacterController = GetComponent<CharacterController>();
        input = GetComponent<InputSystem>();
    }
}
