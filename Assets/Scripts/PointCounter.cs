using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public interface ICommand
{
    void Execute();
    void Undo();
}

public class ScoreCommand : ICommand
{
    private PointCounter pointCounter;
    private int oldScore;
    private int newScore;

    public ScoreCommand(PointCounter pointCounter, int oldScore, int newScore)
    {
        this.pointCounter = pointCounter;
        this.oldScore = oldScore;
        this.newScore = newScore;
    }

    public void Execute()
    {
        pointCounter.SetScore(newScore);
    }

    public void Undo()
    {
        pointCounter.SetScore(oldScore);
    }
}

public class PointCounter : MonoBehaviour
{
    Text text;
    int score = 0;
    private int highScore = 0;
    private string highScoreKey = "HighScore";
    private Stack<ICommand> commandHistory;

    void Start()
    {
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        text = gameObject.GetComponent<Text>();
        commandHistory = new Stack<ICommand>();
        UpdateText();
    }

    public void Increment()
    {
        score++;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(highScoreKey, highScore);
        }

        ICommand command = new ScoreCommand(this, score - 1, score);
        commandHistory.Push(command);
        UpdateText();
    }

    public int Clear()
    {
        int oldScore = score;
        score = 0;
        return oldScore;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("SavedScore", score);
    }

    public void LoadScore()
    {
        if (PlayerPrefs.HasKey("SavedScore"))
        {
            score = PlayerPrefs.GetInt("SavedScore");
        }
    }

    public void Undo()
    {
        if (commandHistory.Count > 0)
        {
            ICommand lastCommand = commandHistory.Pop();
            lastCommand.Undo();
            score--;
            PlayerPrefs.SetInt(highScoreKey, highScore);
            UpdateText();
        }
    }

    private void UpdateText()
    {
        text.text = "Score: " + score + "                 High Score: " + highScore;
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int oldScore)
    {
        score = oldScore;
        UpdateText();
    }
}
