using UnityEngine;

namespace OrbitalSnake.PowerUp
{
    public abstract class TimeBasedPowerUp : BasePowerUp
    {
        [SerializeField] private float PowerUpDuration;
        
        protected int _StartTime;
        
        public int StartTime() => _StartTime;
        public float GetPowerUpDuration() => PowerUpDuration;

        public bool CanDeactivatePowerUp(int currentTime)
        {
            return currentTime - _StartTime > PowerUpDuration;
        }
    }
}