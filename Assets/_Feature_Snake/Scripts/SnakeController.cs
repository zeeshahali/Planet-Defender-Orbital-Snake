using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float steerSpeed = 200f;

    [Header("Boundary Settings")]
    public Vector2 centerPoint = Vector2.zero;
    public float innerRadius = 3f;
    public float outerRadius = 10f;

    private float _steerInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // If using physics, ensure it doesn't rotate via physics engine
        if (rb != null) rb.gravityScale = 0; 
    }

    private void OnEnable() => EventManager.OnSteerInput += HandleSteer;
    private void OnDisable() => EventManager.OnSteerInput -= HandleSteer;
    private void HandleSteer(float value) => _steerInput = value;

    void Update()
    {
        // Handle rotation and movement as usual
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        float rotationAmount = -_steerInput * steerSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, rotationAmount);
    }

    // LateUpdate ensures this is the final word on position for this frame
    void LateUpdate()
    {
        ApplyDonutConstraint();
    }

    private void ApplyDonutConstraint()
    {
        Vector2 currentPos = transform.position;
        float distanceFromCenter = Vector2.Distance(currentPos, centerPoint);

        // If the snake is at the exact center, direction is undefined; 
        // we use transform.up as a fallback to prevent errors.
        Vector2 directionFromCenter = (distanceFromCenter > 0.01f) 
            ? (currentPos - centerPoint).normalized 
            : (Vector2)transform.up;

        if (distanceFromCenter > outerRadius)
        {
            SetPosition(centerPoint + (directionFromCenter * outerRadius));
        }
        else if (distanceFromCenter < innerRadius)
        {
            SetPosition(centerPoint + (directionFromCenter * innerRadius));
        }
    }

    private void SetPosition(Vector2 newPos)
    {
        // If you have a Rigidbody, we must move the Rigidbody position, 
        // otherwise physics will just "pull" the snake back out.
        if (rb != null)
        {
            rb.position = newPos;
        }
        
        transform.position = newPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(centerPoint, outerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centerPoint, innerRadius);
    }
}