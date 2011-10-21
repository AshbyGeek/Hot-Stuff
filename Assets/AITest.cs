using UnityEngine;
using System.Collections;


public class AITest : MonoBehaviour {
	
	
	public GameObject character;
		
	// Use this for initialization
	void Start () {	
	
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 cpos = character.transform.position;
		Vector3 mpos = transform.position;
		
		transform.position = transform.position + 0.01f *(cpos-mpos);
		
	}
}
