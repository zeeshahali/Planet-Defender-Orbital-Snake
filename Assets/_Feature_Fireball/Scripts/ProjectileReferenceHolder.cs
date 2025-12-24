using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "ScriptableObjects/Projectiles/ProjectileReferenceHolder", fileName = "ProjectileReferenceHolder", order = 0)]
public class ProjectileReferenceHolder : ScriptableObject
{
    [SerializeField] private List<Projectile> Projectiles = new List<Projectile>();
    
    public void Clear() => Projectiles.Clear();
    public void Add(Projectile projectile) => Projectiles.Add(projectile);
    public void Remove(Projectile projectile) => Projectiles.Remove(projectile);

    public void FreezeProjectiles()
    {
        foreach (var projectile in Projectiles)
        {
            projectile.UpdateRbConstraints(RigidbodyConstraints.FreezeAll);
            projectile.UpdateMeshRendererState(true);
        }
    }
    
    public void UnFreezeProjectiles()
    {
        foreach (var projectile in Projectiles)
        {
            projectile.UpdateMeshRendererState(false);
            projectile.UpdateRbConstraints(RigidbodyConstraints.None);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ProjectileReferenceHolder))]
public class ProjectileReferenceHolderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ProjectileReferenceHolder system = (ProjectileReferenceHolder)target;

        // Create the button
        if (GUILayout.Button("Freeze Projectiles"))
        {
            system.FreezeProjectiles();
        }
        
        if(GUILayout.Button("Unfreeze Projectiles"))
        {
            system.UnFreezeProjectiles();
        }
    }   
}
#endif