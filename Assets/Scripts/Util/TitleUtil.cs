using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUtil : MonoBehaviour {

	public GameObject titleScreen;
	public GameObject creditScreen;
	public Text gameOver;
	public Text scoreBoard;

	// Use this for initialization
	void Start () {
		titleScreen.SetActive(true);
		creditScreen.SetActive(false);

		gameOver.gameObject.SetActive(ScoreUtil.HasScore);
		scoreBoard.gameObject.SetActive(ScoreUtil.HasScore);
		
		if (ScoreUtil.HasScore) {
			scoreBoard.text = "You survived " + ScoreUtil.GetScore() + "!";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			SingletonFactory.ClearInstances();
			ScoreUtil.ResetScore();
			SceneManager.LoadScene("Overworld");
		}
		if (Input.GetKeyDown(KeyCode.C)) {
			titleScreen.SetActive(!titleScreen.activeSelf);
			creditScreen.SetActive(!titleScreen.activeSelf);
		}
	}
}
