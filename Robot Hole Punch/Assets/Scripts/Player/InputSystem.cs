using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [SerializeField]
    private MouseButtonCode buttonToChargeAndShoot = MouseButtonCode.LeftButton;
    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;
    [SerializeField]
    private KeyCode hookKey = KeyCode.E;
    [SerializeField]
    private KeyCode runKey = KeyCode.LeftShift;

    public bool Charge { get { return Input.GetMouseButtonDown((int)buttonToChargeAndShoot); } }
    public bool Shoot { get { return Input.GetMouseButtonUp((int)buttonToChargeAndShoot); } }
    public bool Run { get { return Input.GetKey(runKey); } }
    public bool Jump { get { return Input.GetKeyDown(jumpKey); } }
    public bool Hook { get { return Input.GetKeyDown(hookKey); } }
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }

    private void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        MouseX = Input.GetAxisRaw("Mouse X");
        MouseY = Input.GetAxisRaw("Mouse Y");
    }

}
