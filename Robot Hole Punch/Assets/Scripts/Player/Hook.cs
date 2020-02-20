using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class Hook : MonoBehaviour
{

    public class HookEvent : UnityEvent<Vector3> { }
    [HideInInspector]
    public HookEvent onHookShotComplete = new HookEvent();
    public float MaxDistance { get; set; } = 10f;
    private LayerMask environmentLayer { get { return LayerManager.Instance.environmentLayer; } }
    private float distanceWent = 0f;

    private new LineRenderer renderer;

    public Transform hookCaller { get; set; }

    private void Start()
    {
        renderer = GetComponent<LineRenderer>();
        renderer.SetPosition(0, hookCaller.position);
    }

    private void Update()
    {
        if (distanceWent > MaxDistance)
        {
            onHookShotComplete.Invoke(Vector3.zero);
            Destroy(gameObject);
        }

        if (Physics.CheckSphere(transform.position, 0.5f, environmentLayer, QueryTriggerInteraction.UseGlobal))
        {
            onHookShotComplete.Invoke(transform.position);
            Destroy(gameObject);
        }

        var fwd = Vector3.back / 2;

        transform.Translate(fwd);
        renderer.SetPosition(0, hookCaller.position);
        renderer.SetPosition(1, transform.position);
        distanceWent += fwd.magnitude;
    }




}
