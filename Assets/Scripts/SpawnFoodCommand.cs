using UnityEngine;

public class SpawnFoodCommand : ICommand
{
    private SpawnFood foodSpawner;
    private Snake snake;

    public SpawnFoodCommand(SpawnFood foodSpawner, Snake snake)
    {
        this.foodSpawner = foodSpawner;
        this.snake = snake;
    }

    public void Execute()
    {
        foodSpawner.Spawn(snake);
    }

    public void Undo()
    {
        if (foodSpawner.currentFood != null)
        {
            GameObject.Destroy(foodSpawner.currentFood);
        }
    }
}