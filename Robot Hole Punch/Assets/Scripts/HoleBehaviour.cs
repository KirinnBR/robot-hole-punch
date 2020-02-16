using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBehaviour : MonoBehaviour
{
    private Collider parent;
    private Transform playerTransform;
    private LayerMask playerLayer;
    [SerializeField]
    private float holeRadius = 3f;

    public static bool hasHole = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) > holeRadius)
        {
            parent.enabled = true;
            Debug.Log("Too far.");
            return;
        }
        parent.enabled = !Physics.CheckSphere(transform.position, holeRadius, playerLayer, QueryTriggerInteraction.Ignore);
    }

    public void Configure(Collider parentCollider, Transform player, LayerMask layer)
    {
        hasHole = true;
        var parentTransform = parentCollider.transform;
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        transform.localScale = new Vector3(holeRadius * 2 / parentTransform.localScale.x, parentTransform.localScale.y + 0.1f, holeRadius * 2 / parentTransform.localScale.z);
        parent = parentCollider;
        playerTransform = player;
        playerLayer = layer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, holeRadius);
    }

}
