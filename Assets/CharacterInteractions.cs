using UnityEngine;
using System.Collections;

public class CharacterInteractions: MonoBehaviour {
	public EnvironmentEngine engine;
	public PrettyTerrain terrain;
	public GameObject characterLoc;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp("Fire1")){
			int[] tile = terrain.tileFromPos(characterLoc.transform.localPosition);
			engine.StartFire(tile[0],tile[1]);
		}
	}
}
