using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnFood : MonoBehaviour
{
    public GameObject foodPrefab;
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    private GameObject currentFood;

    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        Spawn(null);
    }

    public GameObject Spawn(Snake snake)
    {
        int x, y;
        do
        {
            y = (int)Random.Range(borderBottom.position.y, borderTop.position.y);
            x = (int)Random.Range(borderLeft.position.x, borderRight.position.x);
        } while (snake != null && snake.CollidesWith(x, y));

        if (currentFood != null)
        {
            Destroy(currentFood);
        }

        currentFood = Instantiate(foodPrefab, new Vector2(x, y), Quaternion.identity) as GameObject;

        return currentFood;
    }

    public Vector2 GetFoodPosition()
    {
        if (currentFood != null)
        {
            return currentFood.transform.position;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public void SetFoodPosition(Vector2 oldPosition)
    {
        if (currentFood != null)
        {
            currentFood.transform.position = oldPosition;
        }
    }

    public void SaveFoodPosition()
    {
        // Implement saving the food position if desired
    }

    public void LoadFoodPosition()
    {
        // Implement loading the food position if desired
    }
}
