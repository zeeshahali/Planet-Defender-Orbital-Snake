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
        float angle = Random.Range(0f, Mathf.PI * 2);

        // Calculate position using Sine and Cosine
        float x = Mathf.Cos(angle) * EarthConfig.Radius;
        float z = Mathf.Sin(angle) * EarthConfig.Radius;

        Vector3 spawnPos = EarthTransform.position + new Vector3(x, z, 0);
        
        // var spawnPos = EarthTransform.position +  randomDirection * EarthConfig.Radius;
        
        var fireball = Instantiate(FireballPrefab,  spawnPos, Quaternion.identity);
        fireball.Initialize(EarthTransform);
    }
    
    public void SpawnFireballInView()
    {
        Camera mainCam = Camera.main;

        // 1. Pick a random point on the screen (0.0 to 1.0)
        // Adding a small margin (0.1) so they don't pop in exactly at the edge
        float screenX = Random.Range(-0.1f, 1.1f);
        float screenY = Random.Range(-0.1f, 1.1f);

        // 2. Define the distance from the camera
        // We want it to be roughly near the Earth's orbit
        float distanceFromCamera = Vector3.Distance(mainCam.transform.position, EarthTransform.position);
    
        // Add some random depth so they aren't all on a flat plane
        distanceFromCamera += Random.Range(-5f, 5f);

        // 3. Convert Screen point to World point
        Vector3 viewportPoint = new Vector3(screenX, screenY, distanceFromCamera);
        Vector3 spawnPos = mainCam.ViewportToWorldPoint(viewportPoint);

        // 4. Safety Check: Ensure it's not spawning INSIDE the Earth
        // If the spawn point is too close to the center, push it out
        Vector3 dirFromEarth = (spawnPos - EarthTransform.position).normalized;
        float minSpawnRadius = EarthConfig.Radius + 10f;

        if (Vector3.Distance(spawnPos, EarthTransform.position) < minSpawnRadius)
        {
            spawnPos = EarthTransform.position + (dirFromEarth * minSpawnRadius);
        }

        // 5. Instantiate
        var fireball = Instantiate(FireballPrefab, spawnPos, Quaternion.identity);
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
        
        if (GUILayout.Button("Spawn Fireball In View"))
        {
            system.SpawnFireballInView();
        }
    }
}
#endif