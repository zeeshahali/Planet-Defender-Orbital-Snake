using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

namespace OrbitalSnake.PowerUp
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PowerUps/PowerUpsSystem", fileName = "PowerUpsSystem", order = 0)]
    public class PowerUpsSystem : ScriptableObject
    {
        /* List all the available powerUps.
            -- each powerUp will be a SO, it will have its type and a bool which will represent whether it is active or not.
            -- further each powerUp will be an extension, the base will be the same.
         */
        [SerializeField] private List<BasePowerUp> PowerUps;
    
        // List all the active powerUps.
        public List<BasePowerUp> ActivePowerUps = new List<BasePowerUp>();
        
        // cache powerUps for quick activation
        private Dictionary<PowerUpType, BasePowerUp> _powerUpDict = new Dictionary<PowerUpType, BasePowerUp>();
        
        private MonoBehaviour _coroutineRunner;

        private Coroutine _timeCheckCoroutine;
        
        private bool _isGameOver = false;
        
        public void Initialize(MonoBehaviour coroutineRunner)
        {
            _isGameOver = false;
            _coroutineRunner = coroutineRunner;
            
            InitializePowerUpsDictionary();
            
            ActivePowerUps = new List<BasePowerUp>();
            StartTimeCheckCoroutine();
        }

        private void InitializePowerUpsDictionary()
        {
            foreach (BasePowerUp basePowerUp in PowerUps)
            {
                basePowerUp.Initialize();
                _powerUpDict.Add(basePowerUp.PowerUpType, basePowerUp);
            }
        }

        public void ActivatePowerUp(PowerUpType type)
        {
            var powerUp = _powerUpDict[type];
            powerUp.ActivatePowerUp();
            
            if(powerUp is not SingleUsePowerUp)
                ActivePowerUps.Add(powerUp); 
        }

        private void StartTimeCheckCoroutine()
        {
            if (_timeCheckCoroutine != null) return;
            _timeCheckCoroutine = _coroutineRunner.StartCoroutine(TimeBasedPowerUpsCheck());
        }

        private void StopTimeCheckCoroutine()
        {
            if (_timeCheckCoroutine == null) return;
            _coroutineRunner.StopCoroutine(_timeCheckCoroutine);
        }

        private IEnumerator TimeBasedPowerUpsCheck()
        {
            while (!_isGameOver)
            {
                DeactivateTimeBasedPowerUps();
                yield return new WaitForSeconds(1f);
            }
        }

        private void DeactivateTimeBasedPowerUps()
        {
            var currentTime = DateTime.Now.ToEpoch();
            foreach (var powerUp in ActivePowerUps)
            {
                if(powerUp is not TimeBasedPowerUp timeBasedPowerUp) continue;
                if(timeBasedPowerUp.CanDeactivatePowerUp(currentTime))
                    timeBasedPowerUp.DeactivatePowerUp();
            }
        }

        public void GameOver()
        {
            _isGameOver = true;
            StopTimeCheckCoroutine();
        }
    
    }

    public enum PowerUpType
    {
        FreezeTime = 0,
        GrowSnake = 1,
    }
}
