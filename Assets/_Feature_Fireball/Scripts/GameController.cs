using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private FireballSystem FireballSystem;
    [SerializeField] private Transform EarthTransform;
    
    [SerializeField] private SnakeController SnakeController;

    public void Awake()
    {
        FireballSystem.Initialize(EarthTransform, SnakeController);
    }
}
