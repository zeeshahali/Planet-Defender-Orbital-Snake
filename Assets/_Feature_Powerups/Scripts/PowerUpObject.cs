using OrbitalSnake.PowerUp;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    [SerializeField] private PowerUpType PowerUpType;
    [SerializeField] private PowerUpsSystem PowerUpsSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PowerUpsSystem.ActivatePowerUp(PowerUpType);
            Destroy(gameObject);
        }
    }
}
