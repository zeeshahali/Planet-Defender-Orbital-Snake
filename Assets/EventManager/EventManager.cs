using System;
using UnityEngine;

public static class EventManager
{
    public static event Action RequestCameraShake;
    
    public static void TriggerCameraShake() => RequestCameraShake?.Invoke();
}
