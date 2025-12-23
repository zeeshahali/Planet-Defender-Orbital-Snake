using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private FireballSystem FireballSystem;
    [SerializeField] private Planet Planet;
    
    [SerializeField] private SnakeController SnakeController;

    private void Start()
    {
        FireballSystem.Initialize(Planet.PlanetTransform, SnakeController, this);
    }
}
