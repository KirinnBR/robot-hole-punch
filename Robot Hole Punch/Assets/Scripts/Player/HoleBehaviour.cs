using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBehaviour : MonoBehaviour
{
    private Collider parent;
    private Transform player;
    [SerializeField]
    private float holeRadius = 3f;
    [SerializeField]
    private LayerMask playerLayer;

    private static List<bool> validHoles = new List<bool>();
    private int currentHoleIndex = 0;
    private MeshRenderer rend;
    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }
    
    void FixedUpdate()
    {
        Debug.DrawLine(transform.position, player.position, Color.red, 0f, false);
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
        if (Physics.CheckSphere(transform.position, holeRadius, playerLayer, QueryTriggerInteraction.Ignore))
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

    public void Configure(Collider parentCollider, Transform player)
    {
        this.player = player;
        validHoles.Add(false);
        currentHoleIndex = validHoles.Count - 1;
        var parentTransform = parentCollider.transform;
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        transform.localScale = new Vector3(holeRadius * 2 / parentTransform.localScale.x, parentTransform.localScale.y + 0.1f, holeRadius * 2 / parentTransform.localScale.z);
        parent = parentCollider;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, holeRadius);
    }

}
