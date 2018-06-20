using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurretControls : MonoBehaviour {

    public Text healthTxt;
    public int healthCount;
    public Text youLost;

    #region Turret Rotation var's
    public float trackSpeed;
    private Ray ray;
    private float hitdist = .0f;
    public LineRenderer lineR;
    private Plane plane;
    #endregion

    public Transform bulletSpawn;
    public float destroyTimer;

    private float lastFired = Mathf.NegativeInfinity;
    private Coroutine fireRate;
    
    private Turret activeTurret;
    public Turret machineGun;
    public Lazer lazer;
    public RocketLauncher rocketLauncher;

    private void Awake ()
    {
        // Setup starting gun
        SwapGunTo(machineGun);
        plane = new Plane(Vector3.up, transform.position);
        healthTxt.text = "Health: " + healthCount;
    }
	
	private void Update () {

        #region Turret Rotation
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (plane.Raycast(ray, out hitdist))
        {
            Vector3 targetPoint = ray.GetPoint(hitdist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trackSpeed * Time.deltaTime);
            lineR.SetPosition(0, transform.position);
            lineR.SetPosition(1, targetPoint);
        }
        #endregion

        if (Input.GetButtonDown("Fire1"))
        {
            if (fireRate != null)
            {
                StopCoroutine(fireRate);
            }
            fireRate = StartCoroutine(FireRate());
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
        bulletClone.GetComponent<Rigidbody>().velocity = bulletClone.transform.forward * activeTurret.projectileSpeed;
        bulletClone.GetComponent<BulletScript>().hitFunc = activeTurret.OnHit;
        Destroy(bulletClone.gameObject, destroyTimer);
        lastFired = Time.time;
    }

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

    #region GunSwapping
    private void SwapGunTo(Turret turret)
    {
        if (activeTurret != null)
            activeTurret.turretObject.SetActive(false);

        turret.turretObject.SetActive(true);
        activeTurret = turret;
    }
    #endregion

}
