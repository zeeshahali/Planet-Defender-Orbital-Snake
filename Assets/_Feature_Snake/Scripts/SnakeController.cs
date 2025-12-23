using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SnakeController : MonoBehaviour, ILoopDetection
{
    [SerializeField] private SnakeConfig SnakeConfig;
    
    [SerializeField] private SnakeLoopDetector snakeLoopDetector;
    [SerializeField] private DonutBoundary donutBoundary;

    private List<GameObject> _bodyParts = new List<GameObject>();
    private List<Vector3> _positionsHistory = new List<Vector3>();

    private float _steerInput;
    private Rigidbody2D rb;

    private bool _isLooping;

    private bool _canMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.gravityScale = 0;

        _isLooping = false;

        // Initialize snake body
        for (int i = 0; i < SnakeConfig.initialBodySize; i++)
        {
            GrowSnake();
        }
    }

    private void OnEnable()
    {
        EventManager.OnSteerInput += HandleSteer;
        _canMove = true;
    }

    private void OnDisable()
    {
        EventManager.OnSteerInput -= HandleSteer;
        _canMove = false;
    }

    private void HandleSteer(float value) => _steerInput = value;

    public void GameOver()
    {
        _canMove = false;
    }
    
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!_canMove) return;
        
        // 1. Move the Head
        transform.Translate(Vector3.up * SnakeConfig.MoveSpeed * Time.deltaTime);
        
        float targetAngle = -_steerInput * SnakeConfig.SteerSpeed;
        float currentAngle = transform.eulerAngles.z;

        float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * SnakeConfig.SteerLerpSpeed);
        transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
        
        // 2. Handle Constraint (Stay in Donut)
        donutBoundary.ApplyConstraint(this.transform, rb);

        // 3. Track History
        // We insert the current position at the start of the list
        _positionsHistory.Insert(0, transform.position);

        // 4. Move Body Parts
        UpdateBodyPositions();

        // Cleanup History
        if (_positionsHistory.Count > (_bodyParts.Count + 1) * SnakeConfig.gap)
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
            int historyIndex = Mathf.Min((index + 1) * SnakeConfig.gap, _positionsHistory.Count - 1);
            
            Vector3 point = _positionsHistory[historyIndex];
            
            // Move segment to the history point
            body.transform.position = point;

            index++;
        }
    }

    public void GrowSnake()
    {
        GameObject body = SnakeConfig.GetBodyPart(this.transform);
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