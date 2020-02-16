using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour, IDamageable
{
    #region Gun Settings

    [Header("Gun Settings")]

    [SerializeField]
    private LayerMask wallLayer;
    [SerializeField]
    private LayerMask enemiesLayer;
    [SerializeField]
    private GameObject holePrefab;
    [SerializeField]
    private float maxSecondsCharge = 2f;
    private float currentTime = 0f;

    private bool isCharging = false;

    #endregion

    #region Stats Settings

    [Header("Stats Settings")]

    [SerializeField]
    private Stats stats;

    #endregion

    #region References

    private InputSystem input { get { return PlayerCenterControl.Instance.input; } }
    private Camera cam { get { return PlayerCenterControl.Instance.Camera; } }
    public float CurrentHealth { get; private set; }

    #endregion

    #region Common Methods

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProccessInput();
    }

    private void ProccessInput()
    {
        if (!isCharging)
        {
            if (input.Charge)
            {
                currentTime = 0f;
                isCharging = true;
                return;
            }
        }
        else
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 2f)
                Shoot(1f);
            else if (input.Shoot)
                Shoot(currentTime / maxSecondsCharge);
        }
    }

    private void Shoot(float power)
    {
        isCharging = false;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 100f, enemiesLayer, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.TryGetComponent(out IDamageable dmg))
            {
                dmg.TakeDamage(stats.damage * power);
            }
        }
        else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f, wallLayer, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.CompareTag("Destructable"))
            {
                HoleBehaviour hole = Instantiate(holePrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), hit.transform).GetComponent<HoleBehaviour>();
                hole.Configure(hit.collider, transform);
            }
        }
    }

    #endregion

    #region IDamageable Methods

    public void TakeDamage(float amount)
    {

    }

    private void Die()
    {

    }

    #endregion

}
