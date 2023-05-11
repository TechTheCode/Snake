using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointCounter : MonoBehaviour {
	Text text;
	int score = 0;
	private int highScore = 0;
	private string highScoreKey = "HighScore";

	// Use this for initialization
	void Start () {
		highScore = PlayerPrefs.GetInt(highScoreKey, 0);
		text = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Score: " + score + "                 High Score: " + highScore;
	}

	public void increment() {
		score++;
		if (score > highScore)
		{
			highScore = score;
			PlayerPrefs.SetInt(highScoreKey, highScore); // Save the new high score to PlayerPrefs
		}
	}

	public int clear() {
		int oldScore = score;
		score = 0;
		return oldScore;
	}
	public int GetScore()
	{
		return score;
	}

	public void SetScore(int oldScore)
	{
		score = oldScore;
		Update();
	}
}