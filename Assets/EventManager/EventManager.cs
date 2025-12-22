using System;
using UnityEngine;

public static class EventManager
{
    public static event Action RequestCameraShake;
    
    public static void TriggerCameraShake() => RequestCameraShake?.Invoke();
    
    public static event Action<float> OnSteerInput;
    public static void TriggerSteer(float value) => OnSteerInput?.Invoke(value);
}
