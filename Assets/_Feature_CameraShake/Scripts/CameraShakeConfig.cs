using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Camera/Configs/CameraShakeConfig", fileName = "CameraShakeConfig", order = 0)]
public class CameraShakeConfig : ScriptableObject
{
    public float Duration;
    public float Strength;
    public int Vibratio;
    public int Randomness;
    public bool Snapping;
    public bool FadeOut;
}