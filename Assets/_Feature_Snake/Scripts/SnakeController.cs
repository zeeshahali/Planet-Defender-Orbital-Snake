using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour, ILoopDetection
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
    
    [SerializeField] private SnakeLoopDetector snakeLoopDetector;
    [SerializeField] private DonutBoundary donutBoundary;

    private List<GameObject> _bodyParts = new List<GameObject>();
    private List<Vector3> _positionsHistory = new List<Vector3>();

    private float _steerInput;
    private Rigidbody2D rb;

    private bool _isLooping;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.gravityScale = 0;

        _isLooping = false;

        // Initialize snake body
        for (int i = 0; i < initialBodySize; i++)
        {
            GrowSnake();
        }
    }

    private void OnEnable()
    {
        EventManager.OnSteerInput += HandleSteer;
    }

    private void OnDisable()
    {
        EventManager.OnSteerInput -= HandleSteer;
    }

    private void HandleSteer(float value) => _steerInput = value;

    public float steerLerpSpeed = 5f;
    

    void Update()
    {
        Move();
    }

    private void Move()
    {
        // 1. Move the Head
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        
        float targetAngle = -_steerInput * steerSpeed;
        float currentAngle = transform.eulerAngles.z;

        float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * steerLerpSpeed);
        transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
        
        // 2. Handle Constraint (Stay in Donut)
        donutBoundary.ApplyConstraint(this.transform, rb);

        // 3. Track History
        // We insert the current position at the start of the list
        _positionsHistory.Insert(0, transform.position);

        // 4. Move Body Parts
        UpdateBodyPositions();

        // Cleanup History
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

    private void CheckForLoop()
    {
        float collisionThreshold = 0.5f; // Adjust based on snake size

        // Start loop from index 3 or 4 to avoid hitting the 'neck'
        for (int i = 2; i < _bodyParts.Count; i++)
        {
            if (Vector3.Distance(this.transform.position, _bodyParts[i].transform.position) < collisionThreshold)
            {
                Debug.Log("Loop Detected mathematically!");
                _isLooping = true;
                break;
            }
        }
        _isLooping = false;
    }
    
    public bool IsPointInLoop(Vector2 pos)
    {
        return snakeLoopDetector.IsPointInLoop(pos, this.transform.position, _bodyParts);
    }
}