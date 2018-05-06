using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUtil : MonoBehaviour {

	private static float scoreTime = 0;
	public static bool HasScore {
		get { return scoreTime > 0; }
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void SetScore() {
		scoreTime = Time.timeSinceLevelLoad;
	}

	public static void ResetScore() {
		scoreTime = 0;
	}

	public static string GetScore() {
		string score = "";

		int seconds = Mathf.FloorToInt(scoreTime % 60);
		int minutes = Mathf.FloorToInt((scoreTime / 60) % 60);
		int hours = Mathf.FloorToInt((scoreTime / (60 * 60)) % 60);

		if (hours > 0) {
			score += hours.ToString().PadLeft(2, '0') + "h ";
		}

		if (hours > 0 || minutes > 0) {
			score += minutes.ToString().PadLeft(2, '0') + "m ";
		}

		score += seconds.ToString().PadLeft(2, '0') + "s";

		return score;
	}
}
