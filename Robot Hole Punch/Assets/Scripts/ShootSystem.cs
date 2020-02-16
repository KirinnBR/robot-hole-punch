using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShootSystem : MonoBehaviour
{
    [SerializeField]
    private Transform weapon;
    [SerializeField]
    private LayerMask wallLayer;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private GameObject holePrefab;

    private bool isCharging = false;
    private int currentFrame = 0;

    // Update is called once per frame
    void Update()
    {
        if (!isCharging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isCharging = true;
                return;
            }
        }
        else
        {
            currentFrame++;
            if (Input.GetMouseButtonUp(0))
            {
                isCharging = false;
                Shoot();
                currentFrame = 0;
                return;
            }
            if (currentFrame == 120)
            {
                isCharging = false;
                currentFrame = 0;
                Shoot();
            }
        }
        /*
         if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(weapon.position, weapon.forward, out RaycastHit hit, 100f, wallLayer, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.CompareTag("Destructable"))
                {
                    HoleBehaviour hole = Instantiate(holePrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)).GetComponent<HoleBehaviour>();
                    hole.Configure(hit.collider, transform, entitiesLayer);
                }
                else
                {
                    Debug.Log("Non-destroyable obstacle.");
                }
            }
        }
         */
    }

    private void Shoot()
    {
        if (Physics.Raycast(weapon.position, weapon.forward, out RaycastHit hit, 100f, wallLayer, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.CompareTag("Destructable"))
            {
                HoleBehaviour hole = Instantiate(holePrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform).GetComponent<HoleBehaviour>();
                hole.Configure(hit.collider, transform, playerLayer);
                Debug.Log("Hole instantiated.");
            }
            else
            {
                Debug.Log("Non-destroyable obstacle.");
            }
        }
    }

}
