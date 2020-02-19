using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CombatSystem), typeof(InputSystem), typeof(FirstPersonController))]
[RequireComponent(typeof(Animator), typeof(HookSystem), typeof(AudioSource))]
public class PlayerCenterControl : Singleton<PlayerCenterControl>
{
    public InputSystem input { get; private set; }
    [SerializeField]
    private Camera cam;
    public Camera Camera { get { return cam; } }
    public FirstPersonController FirstPersonController { get; private set; }
    public AudioSource Audio { get; private set; }
	public Animator Animator { get; private set; }
    public HookSystem Hook { get; private set; }
    public CombatSystem Combat { get; private set; }

	protected override void Awake()
    {
        base.Awake();
        if (cam == null)
            cam = Camera.main;
        FirstPersonController = GetComponent<FirstPersonController>();
        Animator = GetComponent<Animator>();
        Audio = GetComponent<AudioSource>();
        input = GetComponent<InputSystem>();
        Combat = GetComponent<CombatSystem>();
        Hook = GetComponent<HookSystem>();
    }
}
