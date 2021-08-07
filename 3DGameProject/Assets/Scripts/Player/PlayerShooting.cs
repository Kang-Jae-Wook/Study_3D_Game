using UnityEngine;
using KangJaeWook.SurvivalShooter.Weapons;

namespace KangJaeWook.SurvivalShooter.Characters
{
    public class PlayerShooting : MonoBehaviour
    {
        public BaseWeapon CurrentWeapon;


        private void Update()
        {
            if (Input.GetMouseButton(0)) CurrentWeapon.Shoot();
            if (Input.GetKeyDown(KeyCode.R)) CurrentWeapon.Reload();
        }
    }
}
