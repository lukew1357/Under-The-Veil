using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class ShootEventScript : MonoBehaviour
{
    [SerializeField] private PlayerAimWeapon playerAimWeapon;

    private void Start()
    {
        playerAimWeapon.OnShoot += PlayerAimWeapon_OnShoot;
    }

    private void PlayerAimWeapon_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e)
    {
        UtilsClass.ShakeCamera(.1f, .05f);
    }
}
