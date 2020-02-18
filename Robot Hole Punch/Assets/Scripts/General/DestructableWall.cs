using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableWall : MonoBehaviour
{
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Material stencilMaterial;

    private MeshRenderer rend;

    private Transform reference { get { return PlayerCenterControl.Instance.transform; } }
    private LayerMask holesLayer { get { return LayerManager.Instance.holeLayer; } }
    private LayerMask destructableWallLayer { get { return LayerManager.Instance.destructableEnvironmentLayer; } }

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(reference.position, transform.position) > 100f)
            return;
        if (Physics.Linecast(transform.position, reference.position, holesLayer | destructableWallLayer, QueryTriggerInteraction.Collide))
            rend.material = defaultMaterial;
        else
            rend.material = stencilMaterial;
    }
}
