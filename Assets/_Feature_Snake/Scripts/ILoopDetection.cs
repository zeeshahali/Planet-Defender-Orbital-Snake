using UnityEngine;

public interface ILoopDetection
{
    public bool IsPointInLoop(Vector2 pos);
}

public interface IBodyManipulation
{
    public void GrowSnake();
    public void ShrinkSnake();
}