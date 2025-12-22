using System;
using UnityEngine;

public static class EventManager
{
    public static event Action RequestCameraShake;
    
    public static void TriggerCameraShake() => RequestCameraShake?.Invoke();
    
    public static event Action<float> OnSteerInput;
    public static void TriggerSteer(float value) => OnSteerInput?.Invoke(value);

    public static event Func<Vector2, bool> OnCheckLoopDetection;
    public static bool TriggerCheckLoopDetection(Vector2 direction) => OnCheckLoopDetection != null && OnCheckLoopDetection.Invoke(direction);

    public static event Action OnFireballDestroyed;
    public static void TriggerFireballDestroyed() => OnFireballDestroyed?.Invoke();
}
