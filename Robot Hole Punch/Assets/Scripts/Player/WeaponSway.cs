using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float Amount;
    public float smoothAmount;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }


    void FixedUpdate()
    {
        float movementX = Input.GetAxis("Mouse X") * Amount;
        float movementY = Input.GetAxis("Mouse Y") * Amount;

        Vector3 FinalPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, FinalPosition + initialPosition, Time.deltaTime * smoothAmount);
    }
}
