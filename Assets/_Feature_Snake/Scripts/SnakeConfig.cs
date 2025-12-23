using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Configs/SnakeConfig", fileName = "SnakeConfig", order = 0 )]
public class SnakeConfig : ScriptableObject
{
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;
    public float SteerSpeed = 200f;
    public float SteerLerpSpeed = 5f;
    
    [Header("Body Settings")]
    public GameObject bodyPrefab;
    public int gap = 10; // Frames/steps between segments
    public int initialBodySize = 5;

    public GameObject GetBodyPart(Transform head)
    {
        GameObject body = Instantiate(bodyPrefab, head.position, Quaternion.identity, head);
        return body;
    }
}