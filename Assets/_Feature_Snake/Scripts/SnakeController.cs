using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SnakeController : MonoBehaviour, ILoopDetection, IBodyManipulation
{
    [SerializeField] private SnakeConfig SnakeConfig;
    
    [SerializeField] private SnakeLoopDetector snakeLoopDetector;
    [SerializeField] private DonutBoundary donutBoundary;
    
    [SerializeField] private SnakeReferenceHolder SnakeReferenceHolder;

    private List<GameObject> _bodyParts = new List<GameObject>();
    private List<Vector3> _positionsHistory = new List<Vector3>();

    private float _steerInput;
    private Rigidbody2D rb;
    
    private bool _canMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.gravityScale = 0;
        
        // Initialize snake body
        for (int i = 0; i < SnakeConfig.initialBodySize; i++)
        {
            GrowSnake();
        }

        SnakeReferenceHolder.BodyManipulation = this;
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
    
    private void FixedUpdate()
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

    public void ShrinkSnake()
    {
        var lastBodyPart = _bodyParts[^1];
        _bodyParts.Remove(lastBodyPart);
        Destroy(lastBodyPart);
    }
    
    public bool IsPointInLoop(Vector2 pos)
    {
        return snakeLoopDetector.IsPointInLoop(pos, this.transform.position, _bodyParts);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(SnakeController))]
public class SnakeControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SnakeController system = (SnakeController)target;

        // Create the button
        if (GUILayout.Button("GrowSnake"))
        {
            system.GrowSnake();
        }
        
        if (GUILayout.Button("ShrinkSnake"))
        {
            system.ShrinkSnake();
        }
    }   
}
#endif