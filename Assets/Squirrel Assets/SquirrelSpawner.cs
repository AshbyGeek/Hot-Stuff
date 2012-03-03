using UnityEngine;
using System.Collections;

public class SquirrelSpawner : MonoBehaviour {
	
	public GameObject squirrelModel;
	public int numSquirrels;
	
	// Use this for initialization
	void Start () {
		
		for (int i=0; i<numSquirrels; i++)
		{
			Instantiate(squirrelModel, new Vector3(Random.value * 200 - 100, 20, Random.value*200-100), Quaternion.identity);	
		}
		
	
	}
	
	void OnGUI(){
		GUI.Label(new Rect(20,Screen.height - 70,100,50),numSquirrels.ToString() + " squirrels left");
	}
}
