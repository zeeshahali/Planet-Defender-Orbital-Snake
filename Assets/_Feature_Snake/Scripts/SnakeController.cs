using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float steerSpeed = 200f; // Degrees per second

    private float _steerInput;

    private void OnEnable()
    {
        // Subscribe to the steering event
        EventManager.OnSteerInput += HandleSteer;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        EventManager.OnSteerInput -= HandleSteer;
    }

    private void HandleSteer(float value)
    {
        _steerInput = value;
    }

    void Update()
    {
        // 1. Constant Forward Movement
        // transform.up is used for the XY plane (moving towards the top of the sprite)
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 2. Rotation (Steering)
        // We multiply by -1 because a positive swipe delta usually means turning right
        // In Unity's Z-axis, negative rotation is clockwise (right)
        float rotationAmount = -_steerInput * steerSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, rotationAmount);
    }
}