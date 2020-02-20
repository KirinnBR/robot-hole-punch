using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour, IDamageable
{
    #region Gun Settings

    [Header("Gun Settings")]

    [SerializeField]
    private float maxSecondsCharge = 5f;
    [SerializeField]
    private float reloadAnimationTime = 2.5f;

    private Coroutine reloadCoroutine;
    private bool isCharging = false;
    private bool isReloading = false;
    private float currentTime = 0f;

    public bool CanCharge { get; set; } = true;

    #endregion

    #region Hole Settings

    [Header("Hole Settings")]

    [SerializeField]
    private GameObject holePrefab;
    [SerializeField]
    private float holeMaxRadius = 5f;

    #endregion

    #region Stats Settings

    [Header("Stats Settings")]

    [SerializeField]
    private Stats stats;

    #endregion

    #region Audio Settings

    [Header("Audio Settings")]

    [SerializeField]
    private AudioClip laserCharge;
    [SerializeField]
    private AudioClip laserShot;
    [SerializeField]
    private AudioClip laserReload;


	#endregion

	#region References

	private InputSystem input { get { return PlayerCenterControl.Instance.input; } }
    private Camera cam { get { return PlayerCenterControl.Instance.Camera; } }
    private FirstPersonController firstPersonController { get { return PlayerCenterControl.Instance.FirstPersonController; } }
    private HookSystem hook { get { return PlayerCenterControl.Instance.Hook; } }
    private Animator anim { get { return PlayerCenterControl.Instance.Animator; } }
    public AudioSource audio { get { return PlayerCenterControl.Instance.Audio; } }
    private LayerMask enemiesLayer { get { return LayerManager.Instance.enemyLayer; } }
    private LayerMask holesLayer { get { return LayerManager.Instance.holeLayer; } }
    private LayerMask environmentLayer { get { return LayerManager.Instance.environmentLayer; } }
    private UnityEngine.UI.Slider healthBar { get { return UIManager.Instance.PlayerHealthBar; } }
    public float CurrentHealth { get; private set; }

    #endregion

    #region Common Methods

    private void Start()
    {
        CurrentHealth = stats.health;
        UIManager.Instance.SetLaserChargeIntensity(0f);
        healthBar.maxValue = healthBar.value = CurrentHealth;
    }

    void Update()
    {
        ProccessInput();
        UpdateAnimator();
    }

    private void ProccessInput()
    {
        if (isReloading) return;

        if (!isCharging)
        {
            firstPersonController.CanRun = true;
            if (input.Charge)
            {
                if (!CanCharge) return;

                audio.clip = laserCharge;
                audio.Play();
                hook.CanHook = false;
                firstPersonController.UseSound = false;
                currentTime = 0f;
                isCharging = true;
            }
        }
        else
        {
            firstPersonController.CanRun = false;
            currentTime += Time.deltaTime;
            var intensity = currentTime / maxSecondsCharge;
            UIManager.Instance.SetLaserChargeIntensity(intensity);
            if (currentTime >= maxSecondsCharge)
                Shoot(1f);
            else if (input.Shoot)
                Shoot(intensity);
        }
    }

    private void Shoot(float power)
    {
        isCharging = false;
        RaycastHit hit;

        UIManager.Instance.SetLaserChargeIntensity(0f);

        audio.clip = laserShot;
        audio.PlayOneShot(laserShot);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f, enemiesLayer, QueryTriggerInteraction.UseGlobal))
        {
            if (hit.transform.TryGetComponent(out IDamageable dmg))
            {
                dmg.TakeDamage(stats.damage * power);
            }
            return;
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f, holesLayer, QueryTriggerInteraction.Collide))
        {
            if (Physics.Raycast(hit.point + transform.forward, cam.transform.forward, out hit, 100f, environmentLayer, QueryTriggerInteraction.UseGlobal))
            {
                if (hit.transform.TryGetComponent(out DestructableWall dst))
                    dst.InstantiateHole(holePrefab, holeMaxRadius * power, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));
                //HoleBehaviour hole = Instantiate(holePrefab, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up)).GetComponent<HoleBehaviour>();
            }
        }
        else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f, environmentLayer, QueryTriggerInteraction.UseGlobal))
        {
            if (hit.transform.TryGetComponent(out DestructableWall dst))
                dst.InstantiateHole(holePrefab, holeMaxRadius * power, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));
            //HoleBehaviour hole = Instantiate(holePrefab, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up)).GetComponent<HoleBehaviour>();
        }

        if (reloadCoroutine != null)
            StopCoroutine(reloadCoroutine);
        reloadCoroutine = StartCoroutine(Reload());
    }

    private void UpdateAnimator()
    {
        if (anim == null)
        {
            Debug.Log("Animator == null");
            return;
        }

        anim.SetBool("Is Moving", firstPersonController.IsMoving);
        anim.SetBool("Is Charging", isCharging);
    }

    #endregion

    #region Coroutines

    private float shootEndAnimationTime = 1f;

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(shootEndAnimationTime);
        audio.clip = laserReload;
        audio.Play();
        yield return new WaitForSeconds(reloadAnimationTime);        
        isReloading = false;
        firstPersonController.UseSound = true;
        hook.CanHook = true;
    }

	#endregion

	#region IDamageable Methods

	public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        UIManager.Instance.ChangeHealth(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        CurrentHealth = 0;
        //Call for endgame.
    }

    #endregion

}
