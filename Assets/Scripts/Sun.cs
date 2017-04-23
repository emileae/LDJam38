using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

	public Camera mainCamera;
	public AudioListener mainCameraAudio;
	public Camera zoomedCamera;
	public AudioListener zoomedCameraAudio;

	public float fieldOfView;
	public float minFieldOfView = 10;

	public float zoomedFieldOfView;
	public float zoomedMinFieldOfView = 10;


	private UITexts messageScript;

	public GameObject sight;
	private Bounds sightBounds;

	public GameObject flarePrefab;
	public GameObject antiFlarePrefab;

	public float rotationSpeed = 80;

	public float sunRadius = 10;

	private Transform zoomTarget;

	// Use this for initialization
	void Start () {
		fieldOfView = mainCamera.fieldOfView;
		messageScript = GameObject.Find("Messages").GetComponent<UITexts>();
	}
	
	// Update is called once per frame
	void Update ()
	{

		float input = Input.GetAxisRaw ("Horizontal");
		float inputV = Input.GetAxisRaw ("Vertical");

		if (input > 0) {
			sight.transform.Rotate (Vector3.forward * Time.deltaTime * rotationSpeed, Space.World);
			zoomedCamera.transform.Rotate (Vector3.forward * Time.deltaTime * rotationSpeed, Space.World);
		} else if (input < 0) {
			sight.transform.Rotate (-1 * Vector3.forward * Time.deltaTime * rotationSpeed, Space.World);
			zoomedCamera.transform.Rotate (-1 * Vector3.forward * Time.deltaTime * rotationSpeed, Space.World);
		}

		if (inputV > 0) {
			zoomedCamera.transform.Rotate (-1 * Vector3.right * Time.deltaTime * rotationSpeed * 2, Space.Self);
		} else if (inputV < 0) {
			zoomedCamera.transform.Rotate (Vector3.right * Time.deltaTime * rotationSpeed * 2, Space.Self);
		}

		bool cameraToggleButtonDown = Input.GetButtonDown ("Fire1");
		bool actionButtonDown = Input.GetButtonDown ("Fire3");

		if (actionButtonDown) {
			GameObject flare = (GameObject)Instantiate (flarePrefab, transform.position, Quaternion.Euler (90, 0, 0));
			SunFlare flareScript = flare.GetComponent<SunFlare> ();
			flareScript.velocity = sight.transform.forward;//(new Vector3(sightBounds.max.x, sightBounds.max.y, sightBounds.max.z) - transform.position);

			GameObject antFlare = (GameObject)Instantiate (antiFlarePrefab, transform.position, Quaternion.Euler (90, 0, 0));
			SunFlare antiFlareScript = antFlare.GetComponent<SunFlare> ();
			antiFlareScript.velocity = -sight.transform.forward;//(new Vector3(sightBounds.max.x, sightBounds.max.y, sightBounds.max.z) - transform.position);
		}


		// Camera control
		float mouseWheel = Input.GetAxis ("Mouse ScrollWheel");
		if (mouseWheel > 0f) {
			ZoomIn ();
		} else if (mouseWheel < 0f) {
			ZoomOut ();
		}

		// toggle camera
		if (cameraToggleButtonDown) {
			ToggleCamera ();
		}

//		if (zoomTarget != null) {
////			Debug.Log("Look At: " + zoomTarget.position);
////			zoomedCamera.transform.LookAt(zoomTarget);
//		}

	}

	void ToggleCamera ()
	{
		mainCamera.enabled = !mainCamera.enabled;
		mainCameraAudio.enabled = !mainCameraAudio.enabled;
		zoomedCamera.enabled = !zoomedCamera.enabled;
		zoomedCameraAudio.enabled = !zoomedCameraAudio.enabled;

		if (zoomedCamera.enabled) {
			rotationSpeed = 30;
			Transform planet = messageScript.planets[0].transform.GetChild(0).transform;
			zoomTarget = planet;
			messageScript.OrbitLinesOff();
		}
		if (mainCamera.enabled) {
			rotationSpeed = 80;
			messageScript.OrbitLinesOn();
		}
	}

	void ZoomIn ()
	{
		if (mainCamera.enabled && fieldOfView > minFieldOfView) {
			mainCamera.orthographicSize -= 1f;
			fieldOfView = mainCamera.fieldOfView;
		}

		if (zoomedCamera.enabled &&  zoomedFieldOfView > zoomedMinFieldOfView){
			zoomedCamera.fieldOfView -= 1f;
			zoomedFieldOfView = zoomedCamera.fieldOfView;
		}
	}
	void ZoomOut()
	{
		if (mainCamera.enabled) {
			mainCamera.orthographicSize += 1f;
			fieldOfView = mainCamera.fieldOfView;
		}
		if (zoomedCamera.enabled){
			zoomedCamera.fieldOfView += 1f;
			zoomedFieldOfView = zoomedCamera.fieldOfView;
		}
	}
}
