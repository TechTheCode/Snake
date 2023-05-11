using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnFood : MonoBehaviour {

	// Food Prefab
	public GameObject foodPrefab;

	// Borders
	public Transform borderTop;
	public Transform borderBottom;
	public Transform borderLeft;
	public Transform borderRight;

	// Current Food
	public GameObject currentFood; // <-- Add this

	// Use this for initialization
	void Start () {
		Spawn();
	}

	// Spawn food
	public void Spawn () {
		Spawn(null);
	}

	public GameObject Spawn(Snake snake) {
		int x, y;
		do {
			y = (int) Random.Range(borderBottom.position.y, borderTop.position.y);
			x = (int) Random.Range(borderLeft.position.x, borderRight.position.x);
		} while(snake != null && snake.collidesWith(x, y));

		// Save the reference to the current food
		currentFood = Instantiate(foodPrefab, new Vector2(x, y), Quaternion.identity) as GameObject;
		return currentFood;
	}
	public Vector2 GetFoodPosition()
	{
		// Assuming you have a reference to the current food GameObject
		return currentFood.transform.position;
	}

	public void SetFoodPosition(Vector2 oldPosition)
	{
		// Assuming you have a reference to the current food GameObject
		currentFood.transform.position = oldPosition;
	}
}