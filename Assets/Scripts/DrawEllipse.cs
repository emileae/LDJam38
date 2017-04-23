
// CREDIT: http://answers.unity3d.com/questions/631201/draw-an-ellipse-in-unity-3d.html



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
 [RequireComponent (typeof(LineRenderer))]
 public class DrawEllipse : MonoBehaviour {

	private UITexts messageScript;

 	public bool isHabitable = false;
 	public Material habitableMaterial;
	public Material unInhabitableMaterial;
	public Material cometMaterial;
	public bool isComet = false;

	private LineRenderer lr;

	public int orbitalOrder;
	public int spacing = 10;

	public LayerMask goldilocksLayer;
	public LayerMask hotZoneLayer;

 	public GameObject orbitingBody;

 	public int positionIndex = 0;
	public float speed = 5;
	private float fraction = 0; 

	public float a = 10;// major axis
	public float b = 3;// minor axis
	public float h = 0;// centerx
	public float k = 0;// centery
	public float theta = 0;// rotation of the ellipse  ?? not sure along which axis
	public int resolution = 1000;
 
	private Vector3[] positions;

	private List<Vector3[]> orbits = new List<Vector3[]>();

	// Count the number of consecutive habitable orbits... maybe use it to measure how long life was sustained
	public int habitableOrbits = 0;


	void Awake(){
		lr = GetComponent<LineRenderer> ();
		messageScript = GameObject.Find("Messages").GetComponent<UITexts>();
	}

	public void SetOrbit ()
	{


		a = Random.Range (5 + (orbitalOrder * spacing), 15 + (orbitalOrder * spacing));
		b = Random.Range (3 + (orbitalOrder * spacing), 25 + (orbitalOrder * spacing));

//		a = 6;
//		b = 6;

		theta = Random.Range (0, 90);

		speed = Random.Range (5, 50);

		// comet specific adjustments
		if (isComet) {
			speed *= 3;
			a *= Random.Range (2, 8);// make the orbit a little less circular hopefully
		}

		positions = CreateEllipse (a, b, h, k, theta, resolution);
		lr.SetVertexCount (resolution + 1);
		lr.SetPositions (positions);

		if (isComet) {
//			lr.enabled = false;
			lr.material = cometMaterial;
		}

		positionIndex = Random.Range(0, (positions.Length - 1));
//		positionIndex = 0;
		orbitingBody.transform.position = positions [positionIndex];

		CheckGoldilocks ();

	}

	public void TweakOrbit ()
	{


		positions = CreateEllipse (a, b, h, k, theta, resolution);
		lr.SetPositions (positions);
		CheckGoldilocks ();

	}

	public Vector3[] CreateEllipse (float a, float b, float h, float k, float theta, int resolution)
	{

		positions = new Vector3[resolution + 1];
		Quaternion q = Quaternion.AngleAxis (theta, Vector3.forward);
		Vector3 center = new Vector3 (h, k, 0.0f);

		for (int i = 0; i <= resolution; i++) {
			float angle = (float)i / (float)resolution * 2.0f * Mathf.PI;
			positions [i] = new Vector3 (a * Mathf.Cos (angle), b * Mathf.Sin (angle), 0.0f);
			positions [i] = q * positions [i] + center;
		}
		CheckGoldilocks ();
		return positions;
	}

	void CheckGoldilocks ()
	{

		bool initialState = isHabitable;

		isHabitable = true;
		for (int i = 0; i < 1000; i += 250) {
//			Debug.Log ("Index checked: " + i);
			Collider2D overlapsGoldilocks = Physics2D.OverlapCircle (positions [i], 0.1f, goldilocksLayer);
			Collider2D overlapsHotZone = Physics2D.OverlapCircle (positions [i], 0.1f, hotZoneLayer);

//			Debug.Log ("overlapsGoldilocks: " + overlapsGoldilocks);
//			Debug.Log ("overlapsHotZone: " + overlapsHotZone);

			if (overlapsGoldilocks == null || overlapsHotZone != null) {
				if (initialState) {
					if (!messageScript.tutorial) {
						if (messageScript.habitableWorlds > 0) {
							messageScript.habitableWorlds--;
						}
						messageScript.ShowMessageText ("Life withers and dies");
					}
				}

				isHabitable = false;
			}
		}

		if (!initialState && isHabitable) {
			if (!messageScript.tutorial) {
				messageScript.habitableWorlds++;
				messageScript.ShowMessageText ("From dirt grows the flowers");
			}
		}

		if (isHabitable) {
			gameObject.tag = "Planet";// convert orbiting body to a planet whe it reaches the habitable zone
			if (isComet) {
				isComet = false;
			}
			lr.material = habitableMaterial;
		} else {
			habitableOrbits = 0;
			if (!isComet) {
				lr.material = unInhabitableMaterial;
			}
		}

	}

	void Update ()
	{
		positionIndex = positionIndex % positions.Length;
		int nextPositionIndex = (positionIndex + 1) % positions.Length;
		// Moving the orbitingBody
		if (fraction < 1) {
			fraction += Time.deltaTime * speed;
			orbitingBody.transform.position = Vector3.Lerp (positions [positionIndex], positions [nextPositionIndex], fraction);
		}
		if (fraction >= 1) {
			positionIndex++;
			fraction = 0;
		}

		if (positionIndex == 5 && isHabitable) {
			habitableOrbits++;// this gets reset in the CheckGoldilocks function if planet moves out fo habitable zone
			messageScript.totalHabitableOrbits ++;
			Debug.Log("habitableOrbits:__________" + habitableOrbits);
		}

//		Debug.Log("positionIndex_____________ " + positionIndex);

//		positions = CreateEllipse(a,b,h,k,theta,resolution);
//		LineRenderer lr = GetComponent<LineRenderer>();
//		lr.SetVertexCount (resolution+1);
//		for (int i = 0; i <= resolution; i++) {
//			lr.SetPosition(i, positions[i]);
//		}
	}

 }