using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

[CreateAssetMenu(menuName = "ScriptableObjects/Systems/Fireball System", fileName = "FireballSystem", order = 0)]
public class FireballSystem : ScriptableObject
{
    [SerializeField] private Fireball FireballPrefab;
    [SerializeField] private EarthConfig EarthConfig;
    
    private Transform _earthTransform;
    private ILoopDetection _loopDetection;

    public void Initialize(Transform earthTransform, ILoopDetection loopDetection)
    {
        _earthTransform = earthTransform;
        _loopDetection = loopDetection;
    }

    public void SpawnFireball()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);

        // Calculate position using Sine and Cosine
        float x = Mathf.Cos(angle) * EarthConfig.Radius;
        float y = Mathf.Sin(angle) * EarthConfig.Radius;

        Vector3 spawnPos = _earthTransform.position + new Vector3(x, y, 0);
        
        var fireball = Instantiate(FireballPrefab,  spawnPos, Quaternion.identity);
        fireball.Initialize(_earthTransform);
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
    }
}
#endif