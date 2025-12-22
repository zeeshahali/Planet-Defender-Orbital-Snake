using System.Collections;
using System.Collections.Generic;
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

    [Header("Body Settings")]
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private int gap = 10; // Frames/steps between segments
    [SerializeField] private int initialBodySize = 5;

    private List<GameObject> _bodyParts = new List<GameObject>();
    private List<Vector3> _positionsHistory = new List<Vector3>();

    private float _steerInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.gravityScale = 0;

        // Initialize snake body
        for (int i = 0; i < initialBodySize; i++)
        {
            GrowSnake();
        }
    }

    private void OnEnable() => EventManager.OnSteerInput += HandleSteer;
    private void OnDisable() => EventManager.OnSteerInput -= HandleSteer;
    private void HandleSteer(float value) => _steerInput = value;

    void Update()
    {
        // 1. Move the Head
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        float rotationAmount = -_steerInput * steerSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, rotationAmount);

        // 2. Handle Constraint (Stay in Donut)
        ApplyDonutConstraint();

        // 3. Track History
        // We insert the current position at the start of the list
        _positionsHistory.Insert(0, transform.position);

        // 4. Move Body Parts
        UpdateBodyPositions();

        // 5. Cleanup History (Optional: keeps the list from growing infinitely)
        if (_positionsHistory.Count > (_bodyParts.Count + 1) * gap)
        {
            _positionsHistory.RemoveAt(_positionsHistory.Count - 1);
        }
    }

    private void UpdateBodyPositions()
    {
        int index = 0;
        foreach (var body in _bodyParts)
        {
            // Each segment follows the head's history based on the gap
            // index + 1 because history[0] is the head's current position
            int historyIndex = Mathf.Min((index + 1) * gap, _positionsHistory.Count - 1);
            
            Vector3 point = _positionsHistory[historyIndex];
            
            // Move segment to the history point
            body.transform.position = point;
            
            // Optional: Make segments look at the point they are moving towards
            // body.transform.up = (_positionsHistory[Mathf.Max(0, historyIndex - 1)] - point).normalized;

            index++;
        }
    }

    public void GrowSnake()
    {
        GameObject body = Instantiate(bodyPrefab, transform.position, Quaternion.identity);
        _bodyParts.Add(body);
    }

    private void ApplyDonutConstraint()
    {
        Vector2 currentPos = transform.position;
        float distanceFromCenter = Vector2.Distance(currentPos, centerPoint);

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
        if (rb != null) rb.position = newPos;
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