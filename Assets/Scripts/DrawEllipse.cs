
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

	// Count the number of consecutive habitable orbits... maybe use it to measure how long life was sustained
	private bool logged = false;
	public int habitableOrbits = 0;
	public int currentMilestone = 0;

	// milestone models
	public GameObject treesA;
	public GameObject treesB;
	public GameObject house;
	public GameObject rocket;

	// planet terrain material
	public Material initialMaterial;
	public Material dirtMaterial;
	public Material waterMaterial;
	public Material floraMaterial;
	public Material crazyMaterial;

	public Renderer planetBase;
	public Renderer crater;
	public Renderer terrainA;
	public Renderer terrainB;
	public Renderer terrainC;
	public Renderer terrainD;
	public Renderer terrainE;
	public Renderer terrainF;
	public Renderer terrainG;
	public Renderer terrainH;


	void Awake(){

		lr = GetComponent<LineRenderer> ();
		messageScript = GameObject.Find("Messages").GetComponent<UITexts>();
	}

	void Start(){
		planetBase.material = initialMaterial;
		crater.material = initialMaterial;
	}

	public void SetOrbit ()
	{


		a = Random.Range (2 + (orbitalOrder * spacing), 30 + (orbitalOrder * spacing));
		b = Random.Range (1 + (orbitalOrder * spacing), 40 + (orbitalOrder * spacing));

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
			Collider2D overlapsGoldilocks = Physics2D.OverlapCircle (positions [i], 0.1f, goldilocksLayer);
			Collider2D overlapsHotZone = Physics2D.OverlapCircle (positions [i], 0.1f, hotZoneLayer);

			if (overlapsGoldilocks == null || overlapsHotZone != null) {
				if (initialState) {
					if (!messageScript.tutorial) {
						messageScript.ShowMessageText ("The trees wither and die");
					}
				}

				isHabitable = false;
				habitableOrbits = 0;
				DeactivateAllModels ();
			}
		}

		if (!initialState && isHabitable) {
			if (!messageScript.tutorial) {
				messageScript.ShowMessageText ("From ze dirt grows ze flowers");
			}
		}

		if (isHabitable) {
			gameObject.tag = "Planet";// convert orbiting body to a planet when it reaches the habitable zone
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

		// check if planet is habitable and update score
		messageScript.CheckHabitable();

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

		if (positionIndex == 5 && isHabitable && !logged) {
			logged = true;
			habitableOrbits++;// this gets reset in the CheckGoldilocks function if planet moves out of habitable zone
			messageScript.CheckHabitable();
//			Debug.Log ("habitableOrbits:__________" + habitableOrbits);
		}
		if (positionIndex != 5 && logged) {
			logged = false;
		}

	}

	public void UpdateModel (int milestone)
	{
		currentMilestone = milestone;
//		Debug.Log ("Update Model: " + milestone);

		switch (milestone) {
			case 1:
				if (Random.Range (0, 1) > 0.5f) {
					treesA.SetActive (true);
				} else {
					treesB.SetActive(true);
				}
				SetMaterials();
				break;
			case 5:
				house.SetActive(true);
				break;
			case 15:
				rocket.SetActive(true);
				break;
			case 50:
				break;
			default:
				break;
		}

	}

	public void DeactivateAllModels ()
	{
		treesA.SetActive (false);
		treesB.SetActive (false);
		house.SetActive (false);
		rocket.SetActive (false);
	}

	void SetMaterials(){
		planetBase.material = dirtMaterial;
		crater.material = dirtMaterial;
		terrainA.material = floraMaterial;
		terrainB.material = waterMaterial;
		terrainC.material = waterMaterial;
		terrainD.material = crazyMaterial;
		terrainE.material = waterMaterial;
		terrainF.material = floraMaterial;
		terrainG.material = crazyMaterial;
		terrainH.material = waterMaterial;
	}

 }