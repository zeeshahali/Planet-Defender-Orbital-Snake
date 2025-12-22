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

    private void OnEnable()
    {
        EventManager.OnSteerInput += HandleSteer;
        EventManager.OnCheckLoopDetection += IsFireballInLoop;
    }

    private void OnDisable()
    {
        EventManager.OnSteerInput -= HandleSteer;
    }

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
    
    [Header("Loop Detection")]
    [SerializeField] private float closureThreshold = 1.0f; // Distance to consider a loop "closed"
    [SerializeField] private int minSegmentsForLoop = 10;   // Prevent head from "looping" with its own neck

    // Call this method whenever you want to check a fireball (e.g., every frame or on a timer)
    public bool IsFireballInLoop(Vector2 fireballPos)
    {
        int closureIndex = FindLoopClosureIndex();

        // If no closure index is found, there is no loop
        if (closureIndex == -1) return false;

        // Create a list of points representing the closed loop
        List<Vector2> polygon = new List<Vector2>();
        polygon.Add(transform.position); // The Head

        // Add all body segments from the neck down to the closure point
        for (int i = 0; i <= closureIndex; i++)
        {
            polygon.Add(_bodyParts[i].transform.position);
        }

        return IsPointInPolygon(fireballPos, polygon);
    }

    // Finds the index of the body segment the head is currently "touching"
    private int FindLoopClosureIndex()
    {
        // Skip the first few segments (the neck) to avoid false positives
        for (int i = minSegmentsForLoop; i < _bodyParts.Count; i++)
        {
            float dist = Vector2.Distance(transform.position, _bodyParts[i].transform.position);
            if (dist < closureThreshold)
            {
                return i;
            }
        }
        return -1;
    }

    // The Ray Casting Algorithm (Point-in-Polygon)
    private bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
    {
        bool isInside = false;
        int j = polygon.Count - 1;

        for (int i = 0; i < polygon.Count; i++)
        {
            // Check if the ray crosses the edge between vertex i and vertex j
            if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
                (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
            {
                isInside = !isInside;
            }
            j = i;
        }

        return isInside;
    }

    // Optional: Visualize the closure detection in the editor
    private void OnDrawGizmosSelected()
    {
        int closureIndex = FindLoopClosureIndex();
        if (closureIndex != -1)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_bodyParts[closureIndex].transform.position, closureThreshold);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(centerPoint, outerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centerPoint, innerRadius);
    }
}