using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "ScriptableObjects/Systems/Fireball System", fileName = "FireballSystem", order = 0)]
public class FireballSystem : ScriptableObject
{
    [SerializeField] private EarthConfig EarthConfig;
    [SerializeField] private FireballSpawnConfig FireballSpawnConfig;
    [SerializeField] private bool CanSpawnFireballs;
    
    private Transform _earthTransform;
    private ILoopDetection _loopDetection;

    private MonoBehaviour _coroutineHandler;
    
    private Coroutine _spawningCoroutine;
    
    private ProjectileFactory _projectileFactory;

    public void Initialize(Transform earthTransform, ILoopDetection loopDetection, MonoBehaviour coroutineHandler)
    {
        CanSpawnFireballs = true;

        _projectileFactory = new ProjectileFactory(FireballSpawnConfig.FireballPrefab);
        
        _earthTransform = earthTransform;
        _loopDetection = loopDetection;
        
        _coroutineHandler = coroutineHandler;

        StartSpawningCoroutine();
    }

    private IEnumerator FireballSpawnCoroutine()
    {
        while (CanSpawnFireballs)
        {
            yield return new WaitForSeconds(FireballSpawnConfig.SpawnDelay);
            SpawnFireball();
        }
        StopSpawningCoroutine();
    }

    public void SpawnFireball()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);

        float spawnRadius = EarthConfig.Radius * FireballSpawnConfig.SpawnRadiusMultiplier;
        
        // Calculate position using Sine and Cosine
        float x = Mathf.Cos(angle) * spawnRadius;
        float y = Mathf.Sin(angle) * spawnRadius;

        Vector3 spawnPos = _earthTransform.position + new Vector3(x, y, 0);

        _projectileFactory.CreateFireball(spawnPos, _earthTransform, new GravityMovementStrategy(EarthConfig),
            _loopDetection);
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
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FireballSystem system = (FireballSystem)target;

        // Create the button
        if (GUILayout.Button("Spawn Fireball"))
        {
            system.SpawnFireball();
        }
        
        if(GUILayout.Button("Start Spawning Coroutine"))
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