using UnityEngine;
using System.Collections;

public class BigBang : MonoBehaviour {

	public int numOrbits = 1;
	public GameObject orbit;

	public GameObject[] planetPrefabs;

	public int numComets;
	public GameObject cometOrbit;

	private UITexts messageScript;

	// Use this for initialization
	void Start ()
	{

		Bang ();

//		messageScript = GameObject.Find("Messages").GetComponent<UITexts>();
//
//		for (int i = 0; i < numOrbits; i++) {
////			GameObject prefab = planetPrefabs[Random.Range(0, planetPrefabs.Length)];
//			GameObject o = (GameObject)Instantiate(planetPrefabs[Random.Range(0, planetPrefabs.Length)], Vector3.zero, Quaternion.identity);
//			DrawEllipse orbitScript = o.GetComponent<DrawEllipse>();
//			orbitScript.orbitalOrder = i;
//			orbitScript.SetOrbit();
//			messageScript.planets.Add(orbitScript);
//			messageScript.civillisations.Add(0);
//			messageScript.orbitLines.Add(o.GetComponent<LineRenderer>());
//		}
//
//		for (int i = 0; i < numComets; i++) {
////			GameObject prefab = planetPrefabs[Random.Range(0, planetPrefabs.Length)];
//			GameObject c = (GameObject)Instantiate(planetPrefabs[Random.Range(0, planetPrefabs.Length)], Vector3.zero, Quaternion.identity);
//			DrawEllipse cometScript = c.GetComponent<DrawEllipse>();
//			cometScript.orbitalOrder = 1;// comets can overlap... larger orbits
//			cometScript.isComet = true;// comets can overlap... larger orbits
//			cometScript.SetOrbit();
//			messageScript.planets.Add(cometScript);
//			messageScript.civillisations.Add(0);
//			messageScript.orbitLines.Add(c.GetComponent<LineRenderer>());
//		}

	}

	public void Bang ()
	{
		messageScript = GameObject.Find("Messages").GetComponent<UITexts>();

		for (int i = 0; i < numOrbits; i++) {
//			GameObject prefab = planetPrefabs[Random.Range(0, planetPrefabs.Length)];
			GameObject o = (GameObject)Instantiate(planetPrefabs[Random.Range(0, planetPrefabs.Length)], Vector3.zero, Quaternion.identity);
			DrawEllipse orbitScript = o.GetComponent<DrawEllipse>();
			orbitScript.orbitalOrder = i;
			orbitScript.SetOrbit();
			messageScript.planets.Add(orbitScript);
			messageScript.civillisations.Add(0);
			messageScript.orbitLines.Add(o.GetComponent<LineRenderer>());
		}

		for (int i = 0; i < numComets; i++) {
//			GameObject prefab = planetPrefabs[Random.Range(0, planetPrefabs.Length)];
			GameObject c = (GameObject)Instantiate(planetPrefabs[Random.Range(0, planetPrefabs.Length)], Vector3.zero, Quaternion.identity);
			DrawEllipse cometScript = c.GetComponent<DrawEllipse>();
			cometScript.orbitalOrder = 1;// comets can overlap... larger orbits
			cometScript.isComet = true;// comets can overlap... larger orbits
			cometScript.SetOrbit();
			messageScript.planets.Add(cometScript);
			messageScript.civillisations.Add(0);
			messageScript.orbitLines.Add(c.GetComponent<LineRenderer>());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
