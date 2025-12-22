using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

[CreateAssetMenu(menuName = "ScriptableObjects/Systems/Fireball System", fileName = "FireballSystem", order = 0)]
public class FireballSystem : ScriptableObject
{
    [SerializeField] private Fireball FireballPrefab;
    [SerializeField] private EarthConfig EarthConfig;
    
    private Transform EarthTransform;

    public void Initialize(Transform earthTransform)
    {
        EarthTransform = earthTransform;
    }

    public void SpawnFireball()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        
        float x = Mathf.Cos(angle) * EarthConfig.Radius;
        float y = Mathf.Sin(angle) * EarthConfig.Radius;
        
        var spawnPos = EarthTransform.position +  new Vector3(x, 0, y);
        
        var fireball = Instantiate(FireballPrefab,  spawnPos, Quaternion.identity);
        fireball.Initialize(EarthTransform);
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