using System.Collections;
using OrbitalSnake.PowerUp;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OrbitalSnake.Projectiles
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Systems/ProjectileSystem", fileName = "ProjectileSystem", order = 0)]
    public class ProjectileSystem : ScriptableObject
    {
        [SerializeField] private EarthConfig EarthConfig;
        [SerializeField] private ProjectileSpawnConfig projectileSpawnConfig;
        [SerializeField] private BoundaryConfig BoundaryConfig;
        [SerializeField] private bool CanSpawnFireballs;

        [SerializeField] private ProjectileReferenceHolder ProjectileReferenceHolder;
        [SerializeField] private PowerUpsSystem PowerUpsSystem;

        private Transform _earthTransform;
        
        private ILoopDetection _loopDetection;
        private IBodyManipulation _bodyManipulation;

        private MonoBehaviour _coroutineHandler;

        private Coroutine _spawningCoroutine;

        private ProjectileFactory _projectileFactory;
        
        private Camera _camera;

        public void Initialize(Transform earthTransform, ILoopDetection loopDetection,
            IBodyManipulation bodyManipulation, MonoBehaviour coroutineHandler)
        {
            CanSpawnFireballs = true;
            
            if(_camera == null)
                _camera = Camera.main;

            _projectileFactory = new ProjectileFactory(projectileSpawnConfig.ProjectileSpawnData);

            _earthTransform = earthTransform;
            _loopDetection = loopDetection;
            _bodyManipulation = bodyManipulation;

            _coroutineHandler = coroutineHandler;

            StartSpawningCoroutine();
        }

        private IEnumerator ProjectileSpawnCoroutine()
        {
            ProjectileReferenceHolder.Clear();

            while (CanSpawnFireballs)
            {
                yield return new WaitForSeconds(projectileSpawnConfig.SpawnDelay);
                SpawnProjectile(GetProjectileType());
            }

            StopSpawningCoroutine();
        }

        private ProjectileType GetProjectileType()
        {
            var data = projectileSpawnConfig.ProjectileSpawnData;
            
            // 1. Calculate the sum of all weights
            float totalWeight = 0;
            foreach (var spawnData in data)
            {
                totalWeight += spawnData.SpawnProbability;
            }

            // 2. Pick a random number between 0 and the total weight
            float roll = Random.Range(0f, totalWeight);

            // 3. Iterate and subtract weight until you hit 0
            foreach (var spawnData in data)
            {
                if (roll < spawnData.SpawnProbability)
                {
                    return spawnData.ProjectileType;
                }
        
                roll -= spawnData.SpawnProbability;
            }
            
            return ProjectileType.Fireball;
        }
        
        public void SpawnProjectile(ProjectileType projectileType)
        {
            Vector3 spawnPos = BoundaryConfig.GetRandomConstrainedPosition(_camera, projectileSpawnConfig.SpawnRadiusMultiplier);

            Projectile projectile;
            switch (projectileType)
            {
                case ProjectileType.Snowball:
                    projectile = _projectileFactory.CreateSnowball(spawnPos, _earthTransform, _loopDetection);
                    break;
                case ProjectileType.SpikeBall:
                    projectile = _projectileFactory.CreateSpikeBall(spawnPos, _earthTransform, _loopDetection, _bodyManipulation);
                    break;
                default:
                case ProjectileType.Fireball:
                    projectile = _projectileFactory.CreateFireball(spawnPos, _earthTransform, _loopDetection);
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
            CanSpawnFireballs = true;
            _spawningCoroutine = _coroutineHandler.StartCoroutine(ProjectileSpawnCoroutine());
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
    [CustomEditor(typeof(ProjectileSystem))]
    public class FireballSystemEditor : Editor
    {
        private ProjectileType _projectileType;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ProjectileSystem system = (ProjectileSystem)target;

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("SpawnAnyProjectile", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            _projectileType = (ProjectileType)EditorGUILayout.EnumPopup("Projectile Type", _projectileType);
            if (GUILayout.Button("Spawn Projectile"))
            {
                system.SpawnProjectile(_projectileType);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

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