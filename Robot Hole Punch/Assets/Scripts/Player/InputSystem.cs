using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [SerializeField]
    private MouseButtonCode buttonToChargeAndShoot = MouseButtonCode.LeftButton;
    [SerializeField]
    private KeyCode keyToJump = KeyCode.Space;
    public bool Charge { get { return Input.GetMouseButtonDown((int)buttonToChargeAndShoot); } }
    public bool Shoot { get { return Input.GetMouseButtonUp((int)buttonToChargeAndShoot); } }
    public bool Jump { get { return Input.GetKeyDown(keyToJump); } }
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    private void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
    }

}
