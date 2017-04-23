using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UITexts : MonoBehaviour {

	public Text score;

	public Text textArea;
	private bool slideUp = false;
	public float readSpeed = 3.0f;

	public String[] tutorialMessages;
	public bool tutorial = true;
	private int tutorialIndex = 0;

	public int totalHabitableOrbits = 0;
	public int habitableWorlds = 0;

	// Use this for initialization
	void Start () {
		ShowTutorialText ();
	}

	// Update is called once per frame
	void Update ()
	{

		if (Input.anyKeyDown && tutorialIndex < tutorialMessages.Length){
			ShowTutorialText ();
		}

		score.text = "Total habitable worlds: " + habitableWorlds;

	}

	public void ShowMessageText (String text)
	{
		textArea.text = text;
		StartCoroutine(ClearText ());
	}

	public void ShowTutorialText ()
	{
		textArea.text = tutorialMessages [tutorialIndex];
		tutorialIndex++;
		if (tutorialIndex >= tutorialMessages.Length-1) {
			tutorial = false;
			StartCoroutine(ClearText ());
		}
	}

	IEnumerator ClearText ()
	{
		yield return new WaitForSeconds (readSpeed);
		Debug.Log("Clear text");
		textArea.text = "";
		if (tutorial) {
			ShowTutorialText();
		}
	}
}
