using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Snake/SnakeReferenceHolder", fileName = "SnakeReferenceHolder", order = 0 )]
public class SnakeReferenceHolder : ScriptableObject
{
    public IBodyManipulation BodyManipulation;
}