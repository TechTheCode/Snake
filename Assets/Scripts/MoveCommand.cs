using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class MoveCommand : ICommand
{
    private Snake snake;
    private Vector2 oldPosition;
    private List<Transform> oldTail;

    public MoveCommand(Snake snake)
    {
        this.snake = snake;
    }

    public void Execute()
    {
        oldPosition = snake.transform.position;
        oldTail = new List<Transform>(snake.GetTail());
        snake.Move();
    }

    public void Undo()
    {
        snake.transform.position = oldPosition;
        snake.SetTail(oldTail);
    }
}