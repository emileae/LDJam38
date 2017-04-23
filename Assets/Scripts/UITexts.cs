using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UITexts : MonoBehaviour {

	public AudioSource sunSound;
	public AudioSource planetSound;
	public AudioSource lifeSound;

	public Text score;
	public Text civScore;

	public GameObject instructions;

	public Text textArea;
	private bool slideUp = false;
	public float readSpeed = 3.0f;

	public String[] tutorialMessages;
	public bool tutorial = true;
	private int tutorialIndex = 0;

	public int totalHabitableOrbits = 0;
	public int habitableWorlds = 0;

	public List<DrawEllipse> planets = new List<DrawEllipse>();
	public List<LineRenderer> orbitLines = new List<LineRenderer>();
	public MeshRenderer goldilocksIndicator;

	private List<int> milestones = new List<int>();
	public List<int> civillisations = new List<int>();
	public int highestCivillisation = 0;
	public int currentCivilisationState = 0;
	public String civilisationText = "";

	// Use this for initialization
	void Start () {
		ShowTutorialText ();

		milestones.Add(0);// 0 orbits nuttin'
		milestones.Add(1);// 1 orbits -> habitable and stable
		milestones.Add(5);// 5 orbits get you plants
		milestones.Add(15);// 15 orbits get you houses
		milestones.Add(25);// 25 orbits get you splace age
		milestones.Add(100);// 100 orbits get you apocalypse / end of civillisation

	}

	// Update is called once per frame
	void Update ()
	{

		if (Input.anyKeyDown) {
			if (tutorialIndex < tutorialMessages.Length) {
				ShowTutorialText ();
			}
//			if (instructions.active) {
//				instructions.SetActive(false);
//			}
		}

		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}

		score.text = "Habitable Worlds:" + habitableWorlds;
		civScore.text = civilisationText;

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
		textArea.text = "";
		if (tutorial) {
			ShowTutorialText();
		}
	}

	public void CheckHabitable ()
	{
		habitableWorlds = 0;
		int civilisationState = 0;

		for (int i = 0; i < planets.Count; i++) {
			if (planets [i].isHabitable) {
				habitableWorlds++;
			}
			if (milestones.Contains (planets [i].habitableOrbits)) {
				int milestoneIndex = milestones.IndexOf (planets [i].habitableOrbits);
//				Debug.Log ("Milestone: " + milestoneIndex);
				civillisations [i] = milestoneIndex;
				planets [i].UpdateModel (milestoneIndex);

				if (milestoneIndex > civilisationState) {
					civilisationState = milestoneIndex;
					currentCivilisationState = civilisationState;
				}

				switch (milestones[currentCivilisationState]) {
					case 0:
						civilisationText = "";
						break;
					case 1:
						civilisationText = "Trees";
						break;
					case 5:
						civilisationText = "People";
						break;
					case 15:
						civilisationText = "Space";
						break;
					case 25:
						civilisationText = "The Apocalypse.";
						break;
					case 100:
						civilisationText = "Endurant.";
						break;
					default:
						Debug.Log("fall through UITexts.cs");
						civilisationText = "";
						break;
				}

				if (milestoneIndex > highestCivillisation) {
					highestCivillisation = milestoneIndex;
					switch (milestones[currentCivilisationState]) {
						case 1:
							ShowMessageText ("Trees!");
							LifeSound();
							planets [i].UpdateModel(1);
							break;
						case 5:
							ShowMessageText ("A little house.");
							LifeSound();
							planets [i].UpdateModel(5);
							break;
						case 15:
							ShowMessageText ("They're boldly going where no one has gone before");
							LifeSound();
							planets [i].UpdateModel(15);
							break;
						case 25:
							ShowMessageText ("Its teh end of the world as we know it");
//							Debug.Log("planets [i].gameObject: " + planets [i].gameObject);
//							Debug.Log("planets [i].gameObject.GetComponent<Planet>(): " + planets [i].gameObject.GetComponent<Planet>());
							planets [i].transform.GetChild(0).GetComponent<Planet>().DestroyPlanet();
							break;
						case 100:
							ShowMessageText ("I can't believe you're still here.");
							planets [i].transform.GetChild(0).GetComponent<Planet>().DestroyPlanet();
							break;
						default:
							Debug.Log("fall through UITexts.cs");
							break;
					}
				}
			}
		}
	}

	public void OrbitLinesOff ()
	{
		for (int i = 0; i < orbitLines.Count; i++) {
			orbitLines[i].enabled = false;
		}
//		goldilocksIndicator.enabled = false;
	}

	public void OrbitLinesOn ()
	{
		for (int i = 0; i < orbitLines.Count; i++) {
			orbitLines[i].enabled = true;
		}
//		goldilocksIndicator.enabled = true;
	}

	public void SunSound(){
		Debug.Log("Soundddddddddaaaaa");
		sunSound.Play();
	}
	public void PlanetSound(){
		Debug.Log("Soundddddddddbbbbb");
		planetSound.Play();
	}
	public void LifeSound(){
		Debug.Log("Soundddddddddcccc");
		lifeSound.Play();
	}

}
