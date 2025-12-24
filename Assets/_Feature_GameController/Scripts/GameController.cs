using OrbitalSnake.PowerUp;
using OrbitalSnake.Projectiles;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private ProjectileSystem ProjectileSystem;
    [SerializeField] private PowerUpsSystem PowerUpsSystem;
    
    [SerializeField] private Planet Planet;
    
    [SerializeField] private SnakeController SnakeController;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        PowerUpsSystem.Initialize(this);
        ProjectileSystem.Initialize(Planet.PlanetTransform, SnakeController, 
            SnakeController, this);
    }

    private void OnGameOver()
    {
        ProjectileSystem.GameOver();
        SnakeController.GameOver();
        PowerUpsSystem.GameOver();
    }
    
    private void OnEnable()
    {
        EventManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver -= OnGameOver;
    }
}
