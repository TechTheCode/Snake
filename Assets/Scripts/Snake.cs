using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Snake : MonoBehaviour
{
    Vector2 dir = Vector2.right;
    List<Transform> tail = new List<Transform>();
    public GameObject tailPrefab;
    bool ate = false;
    bool alive = true;

    public PointCounter pointCounter;
    private Stack<ICommand> commandHistory;
    private Vector2 lastFoodPosition;
    private List<Transform> lastTail;

    void Start()
    {
        pointCounter.Clear();
        InvokeRepeating("Move", 0.075f, 0.075f);
        commandHistory = new Stack<ICommand>();
    }

    void Update()
    {
        if (Time.timeScale == 1 && alive)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && dir != Vector2.down)
                dir = Vector2.up;
            else if (Input.GetKeyDown(KeyCode.DownArrow) && dir != Vector2.up)
                dir = Vector2.down;
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && dir != Vector2.right)
                dir = Vector2.left;
            else if (Input.GetKeyDown(KeyCode.RightArrow) && dir != Vector2.left)
                dir = Vector2.right;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Rewind();
        }
    }

    void Move()
    {
        Vector2 oldPosition = transform.position;
        transform.Translate(dir);

        if (ate)
        {
            GameObject newTail = Instantiate(tailPrefab, oldPosition, Quaternion.identity);
            tail.Insert(0, newTail.transform);
            ate = false;
        }
        else if (tail.Count > 0)
        {
            if (tail.Count > 1)
            {
                Vector2 previousPosition = tail[0].position;

                for (int i = 1; i < tail.Count; i++)
                {
                    Vector2 temp = tail[i].position;
                    tail[i].position = previousPosition;
                    previousPosition = temp;

                    if (transform.position == tail[i].position && dir != new Vector2(transform.position.x - tail[i - 1].position.x, transform.position.y - tail[i - 1].position.y))
                    {
                        alive = false;
                    }
                }
            }

            if (transform.position == tail[0].position)
            {
                alive = false;
            }

            tail.Last().position = oldPosition;
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name.StartsWith("FoodPrefab"))
        {
            ate = true;
            pointCounter.Increment();
            Destroy(collider.gameObject);
            GameObject.Find("Main Camera").GetComponent<SpawnFood>().Spawn();
            lastFoodPosition = collider.transform.position;
        }
        else
        {
            alive = false;
        }
    }

    public bool IsAlive()
    {
        return alive;
    }

    public bool CollidesWith(int x, int y)
    {
        bool collided = false;
        if (transform.position.x == x && transform.position.y == y)
        {
            collided = true;
        }
        else
        {
            Transform[] tailArray = tail.ToArray();
            int i = 0;
            while (!collided && i < tailArray.Length)
            {
                Transform t = tailArray[i];
                if (t.position.x == x && t.position.y == y)
                {
                    collided = true;
                }

                i++;
            }
        }

        return collided;
    }

    public List<Transform> GetTail()
    {
        return tail;
    }

    public void SetTail(List<Transform> oldTail)
    {
        tail = oldTail;
    }

    public void SaveGame()
    {
        lastTail = new List<Transform>(tail);
        ICommand tailCommand = new TailCommand(this, lastTail, tail);
        commandHistory.Push(tailCommand);

        SpawnFood spawnFood = GameObject.Find("Main Camera").GetComponent<SpawnFood>();
        spawnFood.SaveFoodPosition();
        lastFoodPosition = spawnFood.GetFoodPosition();

        pointCounter.SaveScore();

        PlayerPrefs.SetInt("SavedGame", 1);
        Debug.Log("Game Saved!");
    }

    public void LoadGame()
    {
        if (PlayerPrefs.GetInt("SavedGame", 0) == 1)
        {
            ICommand tailCommand = commandHistory.Pop();
            tailCommand.Undo();
            tail = new List<Transform>(lastTail);

            SpawnFood spawnFood = GameObject.Find("Main Camera").GetComponent<SpawnFood>();
            spawnFood.LoadFoodPosition();
            spawnFood.SetFoodPosition(lastFoodPosition);

            pointCounter.LoadScore();

            Debug.Log("Game Loaded!");
        }
        else
        {
            Debug.Log("No saved game found!");
        }
    }

    public void Rewind()
    {
        if (commandHistory.Count > 0)
        {
            ICommand tailCommand = commandHistory.Pop();
            tailCommand.Undo();

            SpawnFood spawnFood = GameObject.Find("Main Camera").GetComponent<SpawnFood>();
            spawnFood.LoadFoodPosition();
            spawnFood.SetFoodPosition(lastFoodPosition);

            pointCounter.Undo();

            Debug.Log("Game Rewound!");
        }
    }

    // Inner class for tail command
    private class TailCommand : ICommand
    {
        private Snake snake;
        private List<Transform> oldTail;
        private List<Transform> newTail;

        public TailCommand(Snake snake, List<Transform> oldTail, List<Transform> newTail)
        {
            this.snake = snake;
            this.oldTail = oldTail;
            this.newTail = newTail;
        }

        public void Execute()
        {
            snake.SetTail(newTail);
        }

        public void Undo()
        {
            snake.SetTail(oldTail);
        }
    }
}
