using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform aimGun;
    public Transform firePoint;
    public float fireRate = 2f;
    public float cooldown = 0.5f;
    public int magazineSize = 12;
    public int maxAmmo = 48;
    public int ammo;
    public float reloadTime = 2f;
    public KeyCode reloadKey = KeyCode.R;

    private float lastFireTime;
    private int bulletsInMagazine;

    private bool isReloading;

    void Start()
    {
        ammo = maxAmmo;
        bulletsInMagazine = magazineSize;
    }

    void Update()
    {
        // Aiming
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10; // Distance from the camera
        Vector3 aimDir = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        aimGun.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Shooting
        if (Input.GetMouseButton(0) && Time.time > lastFireTime + cooldown && bulletsInMagazine > 0)
        {
            Shoot();
        }

        // Reloading
        if (Input.GetKeyDown(reloadKey) && !isReloading && bulletsInMagazine < magazineSize && ammo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        lastFireTime = Time.time;
        bulletsInMagazine--;
        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        int bulletsToReload = Mathf.Min(magazineSize - bulletsInMagazine, ammo);
        bulletsInMagazine += bulletsToReload;
        ammo -= bulletsToReload;

        isReloading = false;
    }
}