using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CombatSystem), typeof(InputSystem), typeof(UnityStandardAssets.Characters.FirstPerson.FirstPersonController))]
public class PlayerCenterControl : Singleton<PlayerCenterControl>
{
    public InputSystem input { get; private set; }
    [SerializeField]
    private Camera cam;
    public Camera Camera { get { return cam; } }

    protected override void Awake()
    {
        base.Awake();
        if (cam == null)
            cam = Camera.main;
        input = GetComponent<InputSystem>();
    }
}
