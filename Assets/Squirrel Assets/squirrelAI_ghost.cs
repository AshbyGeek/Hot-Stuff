using UnityEngine;
using System.Collections;

public class squirrelAI_ghost : MonoBehaviour {
	private GameObject[] characters;
	
	public float moveSpeed = 0.2f;
	public float minDist = 1.0f;
	
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
			if (dif.sqrMagnitude < closestDist && dif.sqrMagnitude > minDist){
				closestDist = dif.sqrMagnitude;
				closest = tmp;
			}
		}
		
		if (closest != null){
			transform.LookAt(closest.transform.position,-Vector3.up);
			transform.position += transform.forward * moveSpeed;
		}
	}
}
