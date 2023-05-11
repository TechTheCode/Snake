﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour {

	Snake snake;
	GameObject[] pauseObjects;
	GameObject[] finishObjects;
	CommandInvoker commandInvoker;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
		finishObjects = GameObject.FindGameObjectsWithTag("ShowOnFinish");
		hidePaused();
		hideFinished();

		if(SceneManager.GetActiveScene().name == "MainGame") {
			snake = GameObject.FindGameObjectWithTag("Player").GetComponent<Snake>();
			commandInvoker = GameObject.FindGameObjectWithTag("GameController").GetComponent<CommandInvoker>();
		}
	}

	// Update is called once per frame
	void Update () {
		//if player is dead, end game
		if (Time.timeScale == 1 && snake != null && !snake.isAlive()) {
			finishGame();
		}

		//uses the 'p' button to pause and unpause the game
		if (Input.GetKeyDown (KeyCode.Escape)) {
			togglePaused();
		}

		//uses the 'r' button to undo the last command
		if (Time.timeScale == 0 && Input.GetKeyDown(KeyCode.R)) { // <-- Add this
			commandInvoker.Undo();
		}
	}

	public void togglePaused() {
		if (Time.timeScale == 1) {
			Time.timeScale = 0;
			showPaused ();
		} else if(Time.timeScale == 0) {
			Time.timeScale = 1;
			hidePaused();
		}
	}

	public void showPaused() {
		foreach(GameObject gameObject in pauseObjects) {
			gameObject.SetActive(true);
		}
	}

	public void hidePaused() {
		foreach(GameObject gameObject in pauseObjects) {
			gameObject.SetActive(false);
		}
	}

	public void finishGame() {
		Time.timeScale = 0;
		showFinished();
	}

	public void showFinished() {
		foreach(GameObject gameObject in finishObjects) {
			gameObject.SetActive(true);
		}
	}

	public void hideFinished() {
		foreach (GameObject gameObject in finishObjects) {
			gameObject.SetActive(false);
		}
	}

	public void restartLevel() {
		SceneManager.LoadScene("MainGame");
	}

	public void mainMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	public void quitGame() {
		Application.Quit();
	}
}