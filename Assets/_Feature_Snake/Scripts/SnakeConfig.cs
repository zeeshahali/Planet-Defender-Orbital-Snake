using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Configs/SnakeConfig", fileName = "SnakeConfig", order = 0 )]
public class SnakeConfig : ScriptableObject
{
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;
    public float SteerSpeed = 200f;
    public float SteerLerpSpeed = 5f;
}