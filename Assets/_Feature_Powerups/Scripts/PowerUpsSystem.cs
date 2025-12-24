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
        [SerializeField] private PowerUpSpawnConfig PowerUpSpawnConfig;

        /* List all the available powerUps.
            -- each powerUp will be a SO, it will have its type and a bool which will represent whether it is active or not.
            -- further each powerUp will be an extension, the base will be the same.
         */
        [SerializeField] private List<BasePowerUp> PowerUps;

        // List all the active powerUps.
        public List<BasePowerUp> ActivePowerUps = new List<BasePowerUp>();

        [SerializeField] private bool CanSpawnPowerUps;

        // cache powerUps for quick activation
        private Dictionary<PowerUpType, BasePowerUp> _powerUpDict = new Dictionary<PowerUpType, BasePowerUp>();

        private MonoBehaviour _coroutineRunner;

        private Coroutine _timeCheckCoroutine;
        private Coroutine _spawningCoroutine;

        private bool _isGameOver = false;

        public void Initialize(MonoBehaviour coroutineRunner)
        {
            _isGameOver = false;
            _coroutineRunner = coroutineRunner;

            InitializePowerUpsDictionary();

            ActivePowerUps.Clear();
            StartTimeCheckCoroutine();
        }

        private void InitializePowerUpsDictionary()
        {
            _powerUpDict = new Dictionary<PowerUpType, BasePowerUp>();
            foreach (BasePowerUp basePowerUp in PowerUps)
            {
                basePowerUp.Initialize();
                _powerUpDict.Add(basePowerUp.PowerUpType, basePowerUp);
            }
        }

        /*private IEnumerator PowerUpsSpawnCoroutine()
        {

        }*/

        public void StartSpawningCoroutine()
        {
            if (_spawningCoroutine != null) return;
            CanSpawnPowerUps = true;
            /*_spawningCoroutine = _coroutineRunner.StartCoroutine(PowerUpsSpawnCoroutine());*/
        }

        public void StopSpawningCoroutine()
        {
            if (_spawningCoroutine == null) return;
            _coroutineRunner.StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
            CanSpawnPowerUps = false;
        }

        public void ActivatePowerUp(PowerUpType type)
        {
            var powerUp = _powerUpDict[type];
            powerUp.ActivatePowerUp();

            if (powerUp is not SingleUsePowerUp)
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
            _timeCheckCoroutine = null;
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
                if (powerUp is not TimeBasedPowerUp timeBasedPowerUp) continue;
                if (timeBasedPowerUp.CanDeactivatePowerUp(currentTime))
                    timeBasedPowerUp.DeactivatePowerUp();
            }
        }

        private void DeactivateAllPowerUps()
        {
            foreach (var powerUp in ActivePowerUps)
            {
                powerUp.DeactivatePowerUp();
            }
            ActivePowerUps.Clear();
        }

        public void GameOver()
        {
            _isGameOver = true;
            StopTimeCheckCoroutine();
            DeactivateAllPowerUps();
        }
    }

    public enum PowerUpType
    {
        FreezeTime = 0,
        GrowSnake = 1,
    }

    public class PowerUpSpawnConfig : ScriptableObject
    {
        public List<PowerUpSpawnData> PowerUpSpawnData = new List<PowerUpSpawnData>();

        public PowerUpSpawnData GetPowerUpSpawnData(PowerUpType type)
        {
            var data = PowerUpSpawnData[0];
            foreach (var spawnData in PowerUpSpawnData)
            {
                if (spawnData.PowerUpType != type) continue;
                data = spawnData;
                break;
            }

            return data;
        }
    }

    [Serializable]
    public struct PowerUpSpawnData
    {
        public PowerUpType PowerUpType;
        public float SpawnProbability;

        public void UpdateSpawnProbability(float probability)
        {
            SpawnProbability = probability;
        }
    }
}