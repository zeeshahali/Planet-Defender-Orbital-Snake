using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private FireballSystem FireballSystem;
    [SerializeField] private Transform EarthTransform;

    public void Awake()
    {
        FireballSystem.Initialize(EarthTransform);
    }
}
