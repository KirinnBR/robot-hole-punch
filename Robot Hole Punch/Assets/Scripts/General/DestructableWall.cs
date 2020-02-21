using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestructableWall : MonoBehaviour
{
    [SerializeField]
    private float distanceFromPlayerLimit = 80f;
    [SerializeField]
    private bool alwaysStencil = false;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Material stencilMaterial;

    private Renderer rend;
    private Collider col;

    private Transform reference { get { return PlayerCenterControl.Instance.Camera.transform; } }
    private LayerMask holesLayer { get { return LayerManager.Instance.holeLayer; } }
    private LayerMask environmentLayer { get { return LayerManager.Instance.environmentLayer; } }

    private List<Hole> holes = new List<Hole>();

    private void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        if (alwaysStencil)
        {
            rend.material = stencilMaterial;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Vector3.Distance(reference.position, transform.position) > distanceFromPlayerLimit)
            return;

        foreach (var hole in holes)
        {
            if (hole.IsValid)
            {
                col.enabled = false;
                break;
            }
            col.enabled = true;
        }

        if (alwaysStencil) return;

        if (Physics.Linecast(transform.position, reference.position, holesLayer | environmentLayer, QueryTriggerInteraction.Collide))
            rend.material = defaultMaterial;
        else
            rend.material = stencilMaterial;
    }


    public void InstantiateHole(GameObject hole, float radius, Vector3 hitPosition, Quaternion rotation)
    {
        var pos = new Vector3(hitPosition.x, hitPosition.y, transform.position.z);
        GameObject obj = Instantiate(hole, pos, rotation);
        var newHole = obj.GetComponent<Hole>();
        newHole.Configure(radius, transform.localScale.z);
        holes.Add(newHole);
    }

}
