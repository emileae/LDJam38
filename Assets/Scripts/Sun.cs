using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

	public Camera mainCamera;
	public Camera zoomedCamera;

	public GameObject sight;
	private Bounds sightBounds;

	public GameObject flarePrefab;
	public GameObject antiFlarePrefab;

	public float rotationSpeed = 30;

	public float sunRadius = 10;

	// Use this for initialization
	void Start () {
//		sightBounds = sight.GetComponent<MeshRenderer>().bounds;
//		sight.transform.position = new Vector3(transform.position.x, transform.position.y + sightBounds.extents.y, transform.position.z);

//		LineRenderer lr = GetComponent<LineRenderer> ();
//		lr.SetVertexCount (2);
//		Vector3[] positions = new Vector3[2];
//		positions[0] = transform.position;
//		positions[1] = new Vector3(transform.position.x, transform.position.y + 100, transform.position.z);
//		lr.SetPositions (positions);
	}
	
	// Update is called once per frame
	void Update ()
	{

		float input = Input.GetAxisRaw ("Horizontal");

		if (input > 0) {
			Debug.Log ("Rotate right");
			sight.transform.Rotate (Vector3.forward * Time.deltaTime * rotationSpeed, Space.World);
		} else if (input < 0) {
			Debug.Log ("Rotate left");
			sight.transform.Rotate (-1 * Vector3.forward * Time.deltaTime * rotationSpeed, Space.World);
		}

		bool cameraToggleButtonDown = Input.GetButtonDown ("Fire1");
		bool actionButtonDown = Input.GetButtonDown ("Fire3");

		if (actionButtonDown) {
			Debug.Log ("Shoot flare");
			GameObject flare = (GameObject)Instantiate (flarePrefab, transform.position, Quaternion.Euler (90, 0, 0));
			SunFlare flareScript = flare.GetComponent<SunFlare> ();
			flareScript.velocity = sight.transform.forward;//(new Vector3(sightBounds.max.x, sightBounds.max.y, sightBounds.max.z) - transform.position);

			GameObject antFlare = (GameObject)Instantiate (antiFlarePrefab, transform.position, Quaternion.identity);
			SunFlare antiFlareScript = antFlare.GetComponent<SunFlare> ();
			antiFlareScript.velocity = -sight.transform.forward;//(new Vector3(sightBounds.max.x, sightBounds.max.y, sightBounds.max.z) - transform.position);
		}

		if (cameraToggleButtonDown) {
			ToggleCamera();
		}
		

	}

	void ToggleCamera(){
		mainCamera.enabled = !mainCamera.enabled;
		zoomedCamera.enabled = !zoomedCamera.enabled;
	}
}
