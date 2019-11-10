using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Rigidbody))]
public class CharacterInteractions: MonoBehaviour {
	private EnvironmentEngine engine;
	private PrettyTerrain terrain;
	
	[HideInInspector]
	public TerrainTile curTile;
		
	void Start () {
		curTile = null;
		engine = (EnvironmentEngine) FindObjectOfType(typeof(EnvironmentEngine));
		terrain = (PrettyTerrain) FindObjectOfType(typeof(PrettyTerrain));
	}
	
	void Update(){
		Vector2 tileInd = terrain.tileFromPos(transform.localPosition);
		try{
			curTile = engine.mapgen.tiles[(int)tileInd.x,(int)tileInd.y];
		}catch{
			curTile = null;
		}
	}
	
	public void killCharacter(){
        SceneManager.LoadScene(0);
	}
}
