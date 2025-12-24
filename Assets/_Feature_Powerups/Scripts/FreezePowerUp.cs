using System;
using ExtensionMethods;
using UnityEngine;

namespace OrbitalSnake.PowerUp
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PowerUps/FreezePowerUp", fileName = "FreezePowerUp", order = 0)]
    public class FreezePowerUp : TimeBasedPowerUp
    {
        [SerializeField] private ProjectileReferenceHolder ProjectileReferenceHolder;
        
        public override void ActivatePowerUp()
        {
            base.ActivatePowerUp();
            _StartTime = DateTime.Now.ToEpoch();
            ProjectileReferenceHolder.FreezeProjectiles();
        }

        public override void DeactivatePowerUp()
        {
            base.DeactivatePowerUp();
            ProjectileReferenceHolder.FreezeProjectiles();
        }
    }
}