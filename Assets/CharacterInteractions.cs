using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ProgressBar))]

public class CharacterInteractions: MonoBehaviour {
	public EnvironmentEngine engine;
	public PrettyTerrain terrain;
	public GameObject characterLoc;
	public GameObject fireObj;
	
	public float health;
	public float maxHealth;
	
	private ProgressBar healthBar;
	
	void Start () {
		health = maxHealth;
		healthBar = GetComponent<ProgressBar>();
	}
	
	void Update () {
		//turn the flamethrower on and off
		if (Input.GetButtonDown("Fire1"))
			fireObj.active = true;
		if (Input.GetButtonUp("Fire1"))
			fireObj.active = false;
		
		//tell the engine to add heat to the current tile
		Vector2 tileInd = terrain.tileFromPos(characterLoc.transform.localPosition);
		if (Input.GetButton("Fire1")){
			engine.addFire((int)tileInd.x,(int)tileInd.y);
		}
		
		//if the tile is flaming, reduce the character's health proportional to
		//   the heat of the tile
		TerrainTile tile = engine.mapgen.tiles[(int)tileInd.x,(int)tileInd.y];
		if (tile.isOnFire()){
			health -= (tile.heatIndex - tile.heatThresh)*.5f;
		}
		
		//check if the character has died
		if (health < 0)
		{
			killCharacter();
		}
		
		healthBar.barDisplay = health/maxHealth;
	}
	
	void killCharacter(){
		Application.LoadLevel(0);
	}
}
