using UnityEngine;
using System.Collections;

public class SunFlare : MonoBehaviour {

	public float speed = 30;
	public Vector3 velocity;

	public float timeToDie = 5.0f;

	public ParticleSystem particles;

	// Use this for initialization
	void Start () {
		Quaternion rotation = Quaternion.LookRotation(velocity);
        transform.rotation = rotation;
	}

	// Update is called once per frame
	void Update ()
	{
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
	}

	void OnEnable()
    {
        Debug.Log("PrintOnEnable: script was enabled");
        StartCoroutine(Die());
    }

    IEnumerator Die(){
    	yield return new WaitForSeconds(timeToDie);
    	Debug.Log("Die.....");
//    	gameObject.SetActive(false);
		Destroy(gameObject);
    }
}
