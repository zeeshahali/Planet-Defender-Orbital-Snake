using System.Collections;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

[CreateAssetMenu(menuName = "ScriptableObjects/Systems/Fireball System", fileName = "FireballSystem", order = 0)]
public class FireballSystem : ScriptableObject
{
    [SerializeField] private EarthConfig EarthConfig;
    [SerializeField] private FireballSpawnConfig FireballSpawnConfig;
    
    private Transform _earthTransform;
    private ILoopDetection _loopDetection;

    private MonoBehaviour _coroutineHandler;
    
    private Coroutine _spawningCoroutine;

    public void Initialize(Transform earthTransform, ILoopDetection loopDetection, MonoBehaviour coroutineHandler)
    {
        _earthTransform = earthTransform;
        _loopDetection = loopDetection;
        
        _coroutineHandler = coroutineHandler;

        StartSpawningCoroutine();
    }

    private IEnumerator FireballSpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(FireballSpawnConfig.SpawnDelay);
            SpawnFireball();
        }
    }

    public void SpawnFireball()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);

        // Calculate position using Sine and Cosine
        float x = Mathf.Cos(angle) * EarthConfig.Radius;
        float y = Mathf.Sin(angle) * EarthConfig.Radius;

        Vector3 spawnPos = _earthTransform.position + new Vector3(x, y, 0);
        
        var fireball = Instantiate(FireballSpawnConfig.FireballPrefab,  spawnPos, Quaternion.identity);
        fireball.Initialize(_earthTransform, _loopDetection);
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