using UnityEngine;
using System.Collections;

public class CharacterInteractions: MonoBehaviour {
	public EnvironmentEngine engine;
	public PrettyTerrain terrain;
	public GameObject characterLoc;
	public GameObject fireObj;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
			fireObj.active = true;
		if (Input.GetButtonUp("Fire1"))
			fireObj.active = false;
		
		if (Input.GetButton("Fire1")){
			int[] tile = terrain.tileFromPos(characterLoc.transform.localPosition);
			engine.addFire(tile[0],tile[1]);
		}
	}
}
