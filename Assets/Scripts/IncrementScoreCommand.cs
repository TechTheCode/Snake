using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class IncrementScoreCommand : ICommand
{
    private PointCounter counter;
    private int oldScore;

    public IncrementScoreCommand(PointCounter counter)
    {
        this.counter = counter;
    }

    public void Execute()
    {
        oldScore = counter.GetScore();
        counter.increment();
    }

    public void Undo()
    {
        counter.SetScore(oldScore);
    }
}