using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Rigidbody))]
public class CharacterInteractions: MonoBehaviour {
	private PrettyTerrain terrain;
	
	[HideInInspector]
	public TerrainTile curTile;
		
	void Start () {
		curTile = null;
		terrain = (PrettyTerrain) FindObjectOfType(typeof(PrettyTerrain));
	}
	
	void Update(){
		(int row, int col) = terrain.tileFromPos(transform.localPosition);
		curTile = terrain.tiles[row, col];
	}
	
	public void killCharacter(){
        SceneManager.LoadScene(0);
	}
}
