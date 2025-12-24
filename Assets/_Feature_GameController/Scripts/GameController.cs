using System;
using OrbitalSnake.PowerUp;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private FireballSystem FireballSystem;
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
        FireballSystem.Initialize(Planet.PlanetTransform, SnakeController, this);
    }

    private void OnGameOver()
    {
        FireballSystem.GameOver();
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
