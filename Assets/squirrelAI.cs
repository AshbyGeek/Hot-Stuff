using UnityEngine;
using System.Collections;

public class squirrelAI : MonoBehaviour {
	private GameObject[] characters;
	
	public float moveSpeed = 0.1f;
	
	// Use this for initialization
	void Start () {
		characters = GameObject.FindGameObjectsWithTag("Player");
		Debug.Log("number of players: " + characters.GetLength(0));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GameObject closest = null;
		float closestDist = 99999999999999999999999.0f;
		
		Vector3 targetDir = Vector3.up;
		foreach(GameObject tmp in characters){
			Vector3 dif = tmp.transform.position - transform.position;
			if (dif.sqrMagnitude < closestDist){
				targetDir = dif;
				closestDist = dif.sqrMagnitude;
				closest = tmp;
			}
		}
		
		if (closest != null){
			//rigidbody.velocity = Vector3.forward * moveSpeed;
			targetDir.Normalize();
			rigidbody.transform.LookAt(closest.transform, -Vector3.up);
			//rigidbody.transform.position += targetDir * moveSpeed;
			rigidbody.velocity += targetDir*moveSpeed;
		}
	}
}
