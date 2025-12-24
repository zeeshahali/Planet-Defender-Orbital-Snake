using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private FireballSystem FireballSystem;
    [SerializeField] private Planet Planet;
    
    [SerializeField] private SnakeController SnakeController;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        FireballSystem.Initialize(Planet.PlanetTransform, SnakeController, this);
    }

    private void OnGameOver()
    {
        FireballSystem.GameOver();
        SnakeController.GameOver();
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
