using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurretManager : MonoBehaviour {

    #region UI vars
    public Text healthTxt;
    public int healthCount;
    public Text youLost;
    public Text scoreTxt;
    public int currentScore;
    #endregion

    #region Turret Rotation var's
    public float trackSpeed;
    private Ray ray;
    private float hitdist = .0f;
    public LineRenderer lineR;
    private Plane plane;
    #endregion

    public Transform bulletSpawn;
    public float destroyTimer;

    #region Tower vars
    private float lastFired = Mathf.NegativeInfinity;
    private Coroutine fireRate;
    
    private Turret activeTurret;
    public Turret standardGun;
    public Turret machineGun;
    public Lazer lazer;
    public RocketLauncher rocketLauncher;
    public Swiper swiper;
    #endregion

    private void Awake ()
    {
        // Setup
        SwapGunTo(standardGun);
        plane = new Plane(Vector3.up, transform.position);
        healthTxt.text = "Health: " + healthCount;
        scoreTxt.text = "Score: " + currentScore;
    }
	
	private void Update () {

        #region Turret Rotation
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (plane.Raycast(ray, out hitdist))
        {
            // Get the position on the plane, the mouse is hovering over.
            Vector3 targetPoint = ray.GetPoint(hitdist);
            // Set the target rotation of the turret to lerp towards the target point.
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            // Smoothly rotates the tower towards it's target rotation.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trackSpeed * Time.deltaTime);
            // A linerenderer in-between the turret and the mouse position, to better communicate the position of the mouse and where the player is aiming.
            lineR.SetPosition(0, transform.position);
            lineR.SetPosition(1, targetPoint);
        }
        #endregion

        // Firing the gun
        if (Input.GetButtonDown("Fire1"))
        {
            if (fireRate != null)
            {
                StopCoroutine(fireRate);
            }
            fireRate = StartCoroutine(FireRate());
        }

        #region Swapping of Turrets
        if (Input.GetButtonDown(standardGun.turretSwapKey))
        {
            SwapGunTo(standardGun);
        }
        if (Input.GetButtonDown(machineGun.turretSwapKey))
        {
            SwapGunTo(machineGun);
        }
        if (Input.GetButtonDown(lazer.turretSwapKey))
        {
            SwapGunTo(lazer);
        }
        if (Input.GetButtonDown(rocketLauncher.turretSwapKey))
        {
            SwapGunTo(rocketLauncher);
        }
        if (Input.GetButtonDown(swiper.turretSwapKey))
        {
            SwapGunTo(swiper);
        }
        #endregion
    }

    public void AddScore(int addedScore)
    {
        // Add score
        currentScore += addedScore;
        scoreTxt.text = "Score: " + currentScore;
    }

    public void GetDamaged(int dmg)
    {
        healthCount -= dmg;
        if (healthCount <= 0)
        {
            healthCount = 0;
            youLost.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        healthTxt.text = "Health: " + healthCount;
    }

    #region Turret Fire
    private void Fire()
    {
        ParticleSystem bulletClone;
        bulletClone = Instantiate(activeTurret.projectile, bulletSpawn.position, transform.rotation);
        // Give the spawned bullet velocity towards the aimed direction.
        bulletClone.GetComponent<Rigidbody>().velocity = bulletClone.transform.forward * activeTurret.projectileSpeed;
        // Give the bullet the current tower's effects.
        bulletClone.GetComponent<BulletScript>().hitFunc = activeTurret.OnHit;
        // Destroy the bullet after x time, so it doesn't linger in the scene.
        Destroy(bulletClone.gameObject, destroyTimer);
        // Reset the cooldown on the fire rate.
        lastFired = Time.time;
    }

    #region Firerate/cooldown
    // Accurate firerate countdown to prevent bypassing countdown through gunswapping, or click-spamming.
    private IEnumerator FireRate()
    {
        if (Time.time < lastFired + activeTurret.fireRate)
        {
            yield return new WaitForSeconds(lastFired + activeTurret.fireRate - Time.time);
        }
        while (Input.GetButton("Fire1"))
        {
            Fire();
            yield return new WaitForSeconds(activeTurret.fireRate);
        }
    }
    #endregion
    #endregion

    #region GunSwapping
    private void SwapGunTo(Turret turret)
    {
        if (activeTurret != null)
            activeTurret.turretObject.SetActive(false);

        // Swap current turret with a new one, and set it active.
        turret.turretObject.SetActive(true);
        activeTurret = turret;
    }
    #endregion
}
