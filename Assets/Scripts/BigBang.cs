using UnityEngine;
using System.Collections;

public class BigBang : MonoBehaviour {

	public int numOrbits = 1;
	public GameObject orbit;

	public int numComets;
	public GameObject cometOrbit;

	// Use this for initialization
	void Start ()
	{

		for (int i = 0; i < numOrbits; i++) {
			GameObject o = (GameObject)Instantiate(orbit, Vector3.zero, Quaternion.identity);
			DrawEllipse orbitScript = o.GetComponent<DrawEllipse>();
			orbitScript.orbitalOrder = i;
			orbitScript.SetOrbit();
		}

		for (int i = 0; i < numComets; i++) {
			GameObject c = (GameObject)Instantiate(cometOrbit, Vector3.zero, Quaternion.identity);
			DrawEllipse cometScript = c.GetComponent<DrawEllipse>();
			cometScript.orbitalOrder = 1;// comets can overlap... larger orbits
			cometScript.isComet = true;// comets can overlap... larger orbits
			cometScript.SetOrbit();
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
