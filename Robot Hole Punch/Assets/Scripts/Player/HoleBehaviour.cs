using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshLink))]
public class HoleBehaviour : MonoBehaviour
{
    private Collider parent;
    private Transform player;
    [SerializeField]
    private float radius = 3f;
    [SerializeField]
    private LayerMask playerLayer;

    private static List<bool> validHoles = new List<bool>();
    private int currentHoleIndex = 0;
    private MeshRenderer rend;
    private NavMeshLink link;
    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        link = GetComponent<NavMeshLink>();
    }
    
    void FixedUpdate()
    {
        if (Physics.Linecast(transform.position, player.position, out RaycastHit hit))
        {
            var tag = hit.transform.tag;
            if (tag.Equals("Transparent") || tag.Equals("Destructable"))
            {
                rend.enabled = false;
            }
            else
            {
                rend.enabled = true;
            }
        }
        if (Physics.CheckSphere(transform.position, radius, playerLayer, QueryTriggerInteraction.Ignore))
        {
            validHoles[currentHoleIndex] = true;
            parent.enabled = false;
        }
        else
        {
            validHoles[currentHoleIndex] = false;
            foreach (var validHole in validHoles)
            {
                if (validHole)
                {
                    parent.enabled = false;
                    return;
                }
            }
            parent.enabled = true;
        }
    }

    public void Configure(Collider parentCollider, Transform playerTransform)
    {
        //Setting up references.
        player = playerTransform;
        parent = parentCollider;

        //Setting up HoleBehaviour.
        validHoles.Add(false);
        currentHoleIndex = validHoles.Count - 1;

        //Setting up hole transform.
        transform.localScale = new Vector3(radius * 2, radius * 2, parent.transform.localScale.y + 1f);
        transform.parent = parentCollider.transform;
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);

        //Setting up NavMeshLink.
        var yAxis = player.position.y - transform.position.y - 0.2f;
        if (Mathf.Abs(yAxis) > 5f)
            yAxis = 0f;
        link.startPoint = (Vector3.forward * transform.localScale.z) + (Vector3.up * yAxis);
        link.endPoint = (-Vector3.forward * transform.localScale.z) + (Vector3.up * yAxis);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
