using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class NPC : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Stats stats;

    #region Object Detection Settings

    [Header("Object Detection Settings")]

    [Tooltip("The distance, in meters, of the periferic vision.")]
    public float perifericVisionRadius = 3f;
    [Tooltip("The distance, in meters, of the normal vision.")]
    public float normalVisionRadius = 10f;
    [Tooltip("The angle, in degrees, of the vision.")]
    [Range(0, 360)]
    public float normalVisionAngle = 90f;
    [Tooltip("The distance, in meters, of the vision when the target is defined.")]
    public float wideDistanceVisionRadius = 20f;

    protected List<Transform> visibleObjects = new List<Transform>();
    protected bool hasVisibleObjects { get { return visibleObjects.Count > 0; } }

    #endregion

    #region Agent Settings

    protected NavMeshAgent agent;

    protected bool IsCloseEnoughToPoint(Vector3 point) => Vector3.Distance(transform.position, point) <= agent.stoppingDistance;

	#endregion

	#region References

    protected LayerMask playerLayer { get { return LayerManager.Instance.playerLayer; } }
    protected LayerMask defaultEnvironmentLayer { get { return LayerManager.Instance.defaultEnvironmentLayer; } }
    protected LayerMask destructableEnvironmentLayer { get { return LayerManager.Instance.destructableEnvironmentLayer; } }

    #endregion


    public float CurrentHealth { get; private set; }

    protected virtual void Start()
    {
        CurrentHealth = stats.health;
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void FixedUpdate()
    {
        SearchObjects();
    }
    protected void SearchObjects()
    {
        visibleObjects.Clear();
        var objectsInVisionRadius = Physics.OverlapSphere(transform.position, normalVisionRadius, playerLayer);
        if (objectsInVisionRadius.Length > 0)
        {
            for (int i = 0; i < objectsInVisionRadius.Length; i++)
            {
                Vector3 dirToTarget = (objectsInVisionRadius[i].transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < normalVisionAngle / 2f)
                {
                    if (!Physics.Linecast(transform.position, objectsInVisionRadius[i].transform.position, defaultEnvironmentLayer | destructableEnvironmentLayer))
                    {
                        visibleObjects.Add(objectsInVisionRadius[i].transform);
                    }
                }
            }
        }
        var objectsInShortVisionRadius = Physics.OverlapSphere(transform.position, perifericVisionRadius, playerLayer);
        if (objectsInShortVisionRadius.Length > 0)
        {
            foreach (var obj in objectsInShortVisionRadius)
            {
                if (!visibleObjects.Contains(obj.transform))
                {
                    visibleObjects.Add(obj.transform);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angle, bool isGlobal)
    {
        if (!isGlobal)
            angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(name + " took " + amount + " damage.");
        CurrentHealth -= amount;
        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }
    
    private void Die()
    {
        CurrentHealth = 0f;
        Destroy(gameObject);
    }


#if UNITY_EDITOR
    float lastPeriferic;
    float lastNormal;
    float lastWide;
    private void OnValidate()
    {
        if (perifericVisionRadius != lastPeriferic)
        {
            if (perifericVisionRadius <= 0f)
                perifericVisionRadius = 0f;
            else if (perifericVisionRadius >= normalVisionRadius)
                perifericVisionRadius = normalVisionRadius;
            lastPeriferic = perifericVisionRadius;
        }
        if (normalVisionRadius != lastNormal)
        {
            if (normalVisionRadius <= perifericVisionRadius)
                normalVisionRadius = perifericVisionRadius;
            else if (normalVisionRadius >= wideDistanceVisionRadius)
                normalVisionRadius = wideDistanceVisionRadius;
            lastNormal = normalVisionRadius;
        }
        if (wideDistanceVisionRadius != lastWide)
        {
            if (wideDistanceVisionRadius <= normalVisionRadius)
                wideDistanceVisionRadius = normalVisionRadius;
            lastWide = wideDistanceVisionRadius;
        }
    }
#endif

}
