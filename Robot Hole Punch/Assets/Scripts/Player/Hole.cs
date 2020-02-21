using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshLink))]
public class Hole : MonoBehaviour
{
    public float Radius { get; set; }
    public bool IsValid { get; set; } = false;

    private Renderer rend;
    private Transform player { get { return PlayerCenterControl.Instance.Camera.transform; } }
    private LayerMask environmentLayer { get { return LayerManager.Instance.environmentLayer; } }
    private LayerMask playerLayer { get { return LayerManager.Instance.playerLayer; } }

    private NavMeshLink link;
    private void Awake()
    {
        link = GetComponent<NavMeshLink>();
        rend = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        IsValid = Physics.CheckSphere(transform.position, Radius, playerLayer, QueryTriggerInteraction.Ignore);
        if (IsValid)
        {
            rend.enabled = true;
            return;
        }
        rend.enabled = !Physics.Linecast(transform.position, player.position, environmentLayer, QueryTriggerInteraction.UseGlobal);
    }

    public void Configure(float radius, float zScale)
    {
        Radius = radius;

        transform.localScale = new Vector3(radius * 2, radius * 2, zScale);

        //Setting up NavMeshLink.
        if (Physics.Raycast(transform.position + Vector3.forward * (zScale + 1f), Vector3.down, out RaycastHit hit, 5f, environmentLayer, QueryTriggerInteraction.Ignore))
        {
            var yAxis = hit.distance;
            link.startPoint = (Vector3.forward * (zScale + 0.5f)) + (Vector3.down * yAxis);
            link.endPoint = (-Vector3.forward * (zScale + 0.5f)) + (Vector3.down * yAxis);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, Radius);
    }

}
