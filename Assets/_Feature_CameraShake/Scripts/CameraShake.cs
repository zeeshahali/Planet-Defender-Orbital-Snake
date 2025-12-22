using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private CameraShakeConfig CameraShakeConfig;

    public void Shake()
    {
        Target.DOShakePosition(CameraShakeConfig.Duration, CameraShakeConfig.Strength, CameraShakeConfig.Vibratio,
            CameraShakeConfig.Randomness, CameraShakeConfig.Snapping, CameraShakeConfig.FadeOut);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(CameraShake))]
public class CameraShakeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CameraShake camera = (CameraShake)target;

        // Create the button
        if (GUILayout.Button("Shake"))
        {
            camera.Shake();
        }
    }
}

#endif