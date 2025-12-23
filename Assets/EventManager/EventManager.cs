using System;
using UnityEngine;

public static class EventManager
{
    public static event Action OnCollisionWithPlanet;
    
    public static void TriggerCollisionWithPlanet() => OnCollisionWithPlanet?.Invoke();
    
    public static event Action<float> OnSteerInput;
    public static void TriggerSteer(float value) => OnSteerInput?.Invoke(value);

    public static event Func<Vector2, bool> OnCheckLoopDetection;
    public static bool TriggerCheckLoopDetection(Vector2 direction) => OnCheckLoopDetection != null && OnCheckLoopDetection.Invoke(direction);

    public static event Action OnFireballDestroyed;
    public static void TriggerFireballDestroyed() => OnFireballDestroyed?.Invoke();

    public static event Action<float> OnPlanetHealthUpdate;
    public static void TriggerPlanetHealthUpdate(float value) => OnPlanetHealthUpdate?.Invoke(value);
    
    public static event Action OnGameOver;
    public static void TriggerGameOver() => OnGameOver?.Invoke();
}
