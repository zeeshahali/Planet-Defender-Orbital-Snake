using UnityEngine;

namespace OrbitalSnake.PowerUp
{
    public abstract class TimeBasedPowerUp : BasePowerUp
    {
        [SerializeField] protected float PowerUpDuration;
        
        protected int _StartTime;
        
        public int StartTime() => _StartTime;
        public float GetPowerUpDuration() => PowerUpDuration;
        public void SetPowerUpDuration(float duration) => PowerUpDuration = duration;

        public bool CanDeactivatePowerUp(int currentTime)
        {
            return currentTime - _StartTime > PowerUpDuration;
        }
    }
}