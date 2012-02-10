using UnityEngine;
using System.Collections;

public class squirrelAI : MonoBehaviour {
	private GameObject[] characters;
	
	public float moveSpeed = 1.0f;
	
	// Use this for initialization
	void Start () {
		characters = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		GameObject closest = null;
		float closestDist = 99999999999999999999999.0f;
		foreach(GameObject tmp in characters){
			Vector3 dif = tmp.transform.position - transform.position;
			if (dif.sqrMagnitude < closestDist){
				closestDist = dif.sqrMagnitude;
				closest = tmp;
			}
		}
		
		if (closest != null){
			rigidbody.velocity = Vector3.forward * moveSpeed;
		}
	}
}
