using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CombatSystem), typeof(InputSystem), typeof(FirstPersonController))]
[RequireComponent(typeof(Animator))]
public class PlayerCenterControl : Singleton<PlayerCenterControl>
{
    public InputSystem input { get; private set; }
    [SerializeField]
    private Camera cam;
    public Camera Camera { get { return cam; } }
    public CharacterController CharacterController { get; private set; }
    public FirstPersonController FirstPersonController { get; private set; }
	public Animator Animator { get; private set; }

	protected override void Awake()
    {
        base.Awake();
        if (cam == null)
            cam = Camera.main;
        CharacterController = GetComponent<CharacterController>();
        FirstPersonController = GetComponent<FirstPersonController>();
        Animator = GetComponent<Animator>();
        input = GetComponent<InputSystem>();
    }
}
