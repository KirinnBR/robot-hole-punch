using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    private bool isColliding = false;
    Vector3 dir;

    public void Shoot(Vector3 dir)
    {
        this.dir = dir;
        StartCoroutine(DoShoot());
    }

    private IEnumerator DoShoot()
    {
        var touch = Physics.CheckSphere(transform.position, 1f);
        while (touch)
        {
            touch = Physics.CheckSphere(transform.position, 1f);
            Debug.Log(touch);
            transform.position += dir / 3;
            yield return null;
        }
    }

}
