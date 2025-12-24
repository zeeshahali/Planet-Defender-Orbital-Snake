using System;
using System.Collections;
using OrbitalSnake.PowerUp;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OrbitalSnake.Projectiles
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Systems/Fireball System", fileName = "FireballSystem", order = 0)]
    public class FireballSystem : ScriptableObject
    {
        [SerializeField] private EarthConfig EarthConfig;
        [SerializeField] private FireballSpawnConfig FireballSpawnConfig;
        [SerializeField] private bool CanSpawnFireballs;

        [SerializeField] private ProjectileReferenceHolder ProjectileReferenceHolder;
        [SerializeField] private PowerUpsSystem PowerUpsSystem;

        private Transform _earthTransform;
        
        private ILoopDetection _loopDetection;
        private IBodyManipulation _bodyManipulation;

        private MonoBehaviour _coroutineHandler;

        private Coroutine _spawningCoroutine;

        private ProjectileFactory _projectileFactory;

        public void Initialize(Transform earthTransform, ILoopDetection loopDetection,
            IBodyManipulation bodyManipulation, MonoBehaviour coroutineHandler)
        {
            CanSpawnFireballs = true;

            _projectileFactory = new ProjectileFactory(FireballSpawnConfig.FireballPrefab, 
                FireballSpawnConfig.SpikeBallPrefab, FireballSpawnConfig.SnowballPrefab);

            _earthTransform = earthTransform;
            _loopDetection = loopDetection;
            _bodyManipulation = bodyManipulation;

            _coroutineHandler = coroutineHandler;

            StartSpawningCoroutine();
        }

        private IEnumerator FireballSpawnCoroutine()
        {
            ProjectileReferenceHolder.Clear();

            while (CanSpawnFireballs)
            {
                yield return new WaitForSeconds(FireballSpawnConfig.SpawnDelay);
                SpawnProjectile(ProjectileType.Fireball);
            }

            StopSpawningCoroutine();
        }
        
        public void SpawnProjectile(ProjectileType projectileType)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);

            float spawnRadius = EarthConfig.Radius * FireballSpawnConfig.SpawnRadiusMultiplier;

            // Calculate position using Sine and Cosine
            float x = Mathf.Cos(angle) * spawnRadius;
            float y = Mathf.Sin(angle) * spawnRadius;

            Vector3 spawnPos = _earthTransform.position + new Vector3(x, y, 0);

            Projectile projectile;
            GravityMovementStrategy gravityMovementStrategy = new GravityMovementStrategy(EarthConfig);

            switch (projectileType)
            {
                case ProjectileType.Snowball:
                    projectile = _projectileFactory.CreateSnowball(spawnPos, _earthTransform,
                        gravityMovementStrategy, _loopDetection);
                    break;
                case ProjectileType.SpikeBall:
                    projectile = _projectileFactory.CreateSpikeBall(spawnPos, _earthTransform,
                        gravityMovementStrategy, _loopDetection, _bodyManipulation);
                    break;
                default:
                case ProjectileType.Fireball:
                    projectile = _projectileFactory.CreateFireball(spawnPos, _earthTransform,
                        gravityMovementStrategy, _loopDetection);
                    break;
            }

            ProjectileReferenceHolder.Add(projectile);
            CheckActivePowerUps(projectile);
        }

        private void CheckActivePowerUps(Projectile projectile)
        {
            foreach (var activePowerUp in PowerUpsSystem.ActivePowerUps)
            {
                if (!activePowerUp.IsPowerUpActive || activePowerUp is not TimeBasedPowerUp timeBasedPowerUp) continue;
                if (timeBasedPowerUp.PowerUpType == PowerUpType.FreezeTime)
                {
                    projectile.UpdateMeshRendererState(true);
                    projectile.UpdateRbConstraints(RigidbodyConstraints.FreezeAll);
                }
            }
        }

        public void StartSpawningCoroutine()
        {
            if (_spawningCoroutine != null) return;
            _spawningCoroutine = _coroutineHandler.StartCoroutine(FireballSpawnCoroutine());
        }

        public void StopSpawningCoroutine()
        {
            if (_spawningCoroutine == null) return;
            _coroutineHandler.StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
            CanSpawnFireballs = false;
        }

        public void GameOver()
        {
            StopSpawningCoroutine();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(FireballSystem))]
    public class FireballSystemEditor : Editor
    {
        private ProjectileType _projectileType;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            FireballSystem system = (FireballSystem)target;

            EditorGUILayout.BeginHorizontal();
            _projectileType = (ProjectileType)EditorGUILayout.EnumPopup("Projectile Type", _projectileType);

            // Create the button
            if (GUILayout.Button("Spawn Fireball"))
            {
                system.SpawnProjectile(_projectileType);
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Start Spawning Coroutine"))
            {
                system.StartSpawningCoroutine();
            }

            if (GUILayout.Button("Stop Spawning Coroutine"))
            {
                system.StopSpawningCoroutine();
            }
        }
    }
#endif
}