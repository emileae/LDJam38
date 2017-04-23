using UnityEngine;
using System.Collections;
using UnityEditor.VersionControl;

public class Planet : MonoBehaviour {

	private UITexts messageScript;

	public DrawEllipse orbitScript;

	public ParticleSystem particleExplosion;

	private bool dead = false;


	// Use this for initialization
	void Start () {
		messageScript = GameObject.Find("Messages").GetComponent<UITexts>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (dead && !particleExplosion.IsAlive ()) {
			Destroy(transform.parent.gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{


		if (!dead) {

			// pushes orbits away
			if (col.CompareTag ("Flare")) {
				Debug.Log ("HIT!!!! change orbit.... " + orbitScript.positionIndex);
			
				if (orbitScript.positionIndex > 125 && orbitScript.positionIndex < 375) {
//				Debug.Log("Increase b");
					orbitScript.b += 0.5f;
				}
				if (orbitScript.positionIndex > 625 && orbitScript.positionIndex < 875) {
//				Debug.Log("Increase b");
					orbitScript.b += 0.5f;
				}
				if (orbitScript.positionIndex > 875 || orbitScript.positionIndex < 125) {
//				Debug.Log("Increase a");
					orbitScript.a += 0.5f;
				}
				if (orbitScript.positionIndex > 375 && orbitScript.positionIndex < 625) {
//				Debug.Log("Increase a");
					orbitScript.a += 0.5f;
				}

				orbitScript.CreateEllipse (orbitScript.a, orbitScript.b, orbitScript.h, orbitScript.k, orbitScript.theta, orbitScript.resolution);
				orbitScript.TweakOrbit ();
			}
			// brings orbits closer
			if (col.CompareTag ("AntiFlare")) {
			
				if (orbitScript.positionIndex > 125 && orbitScript.positionIndex < 375) {
					orbitScript.b -= 0.5f;
				}
				if (orbitScript.positionIndex > 625 && orbitScript.positionIndex < 875) {
					orbitScript.b -= 0.5f;
				}
				if (orbitScript.positionIndex > 875 || orbitScript.positionIndex < 125) {
					orbitScript.a -= 0.5f;
				}
				if (orbitScript.positionIndex > 375 && orbitScript.positionIndex < 625) {
					orbitScript.a -= 0.5f;
				}

				orbitScript.CreateEllipse (orbitScript.a, orbitScript.b, orbitScript.h, orbitScript.k, orbitScript.theta, orbitScript.resolution);
				orbitScript.TweakOrbit ();
			}

			// ? maybe upsets orbital direction somehow, or just destroys life on the planet
			if (col.CompareTag ("Comet") || col.CompareTag ("Planet")) {

				if (!messageScript.tutorial) {
					messageScript.ShowMessageText ("Worlds collide");
				}

				particleExplosion.Play ();

				float randomAdjustment = Random.Range (-2.0f, 2.0f);

				if (orbitScript.positionIndex > 125 && orbitScript.positionIndex < 375) {
					orbitScript.b += randomAdjustment;
				}
				if (orbitScript.positionIndex > 625 && orbitScript.positionIndex < 875) {
					orbitScript.b += randomAdjustment;
				}
				if (orbitScript.positionIndex > 875 || orbitScript.positionIndex < 125) {
					orbitScript.a += randomAdjustment;
				}
				if (orbitScript.positionIndex > 375 && orbitScript.positionIndex < 625) {
					orbitScript.a += randomAdjustment;
				}

//				orbitScript.habitableOrbits = 0;
//				orbitScript.isHabitable = false;

//				orbitScript.CreateEllipse (orbitScript.a, orbitScript.b, orbitScript.h, orbitScript.k, orbitScript.theta, orbitScript.resolution);
				orbitScript.TweakOrbit ();

			}

			if (col.CompareTag ("Sun")) {
//			Destroy(transform.parent.gameObject);

				if (!messageScript.tutorial) {
					messageScript.ShowMessageText ("A World is consumed");
				}

				particleExplosion.Play ();
				dead = true;

			}

		}

	}
}
