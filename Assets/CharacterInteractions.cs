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
	
	void Update(){
	}
	
	void FixedUpdate () {
		//turn the flamethrower on and off
		if (Input.GetButtonDown("Fire1"))
			fireObj.active = true;
		if (Input.GetButtonUp("Fire1"))
			fireObj.active = false;
		
		//tell the engine to add heat to the current tile, but only if we're still inside the
		//  tiles area
		Vector2 tileInd = terrain.tileFromPos(characterLoc.transform.localPosition);
		if (tileInd.x < engine.mapgen.rows && tileInd.y < engine.mapgen.cols &&
		    tileInd.x >= 0 && tileInd.y >= 0){
			if (Input.GetButton("Fire1")){
				engine.addFire((int)tileInd.x,(int)tileInd.y);
			}
			
			TerrainTile tile = engine.mapgen.tiles[(int)tileInd.x,(int)tileInd.y];
			if (tile.isOnFire()){
				health -= 1;
			}
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
