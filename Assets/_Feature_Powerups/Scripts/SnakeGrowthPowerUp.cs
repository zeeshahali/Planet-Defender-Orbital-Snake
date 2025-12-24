using UnityEngine;

namespace OrbitalSnake.PowerUp
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PowerUps/SnakeGrowthPowerUp", fileName = "SnakeGrowthPowerUp", order = 0)]
    public class SnakeGrowthPowerUp : SingleUsePowerUp
    {
        [SerializeField] private SnakeReferenceHolder SnakeReferenceHolder;

        public override void ActivatePowerUp()
        {
            SnakeReferenceHolder.BodyManipulation.GrowSnake();
        }
    }
}