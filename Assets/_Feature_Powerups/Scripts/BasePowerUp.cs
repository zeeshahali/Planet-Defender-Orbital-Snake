using UnityEngine;

namespace OrbitalSnake.PowerUp
{
    public abstract class BasePowerUp : ScriptableObject
    {
        public PowerUpType PowerUpType;
        public bool IsPowerUpActive;

        public void Initialize()
        {
            IsPowerUpActive = false;
        }
        
        public virtual void ActivatePowerUp()
        {
            if (IsPowerUpActive) return;

            IsPowerUpActive = true;
        }

        public virtual void DeactivatePowerUp()
        {
            if (!IsPowerUpActive) return;

            IsPowerUpActive = false;
        }
    }
}
