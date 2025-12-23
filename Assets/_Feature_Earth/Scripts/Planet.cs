using System;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private EarthConfig config;
    [SerializeField] private Transform planetTransform;

    public Transform PlanetTransform => planetTransform;

    private void Awake()
    {
        planetTransform.localScale = Vector3.one * config.Radius * 2;
    }
}