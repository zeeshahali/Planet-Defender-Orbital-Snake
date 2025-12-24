using UnityEngine;

namespace OrbitalSnake.Projectiles
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Projectiles/ProjectileConfig", fileName = "_Config", order = 0)]
    public class ProjectileConfig : ScriptableObject
    {
        public float Speed;
        public float Damage;
    }
}